using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SiloAI.Application.Shared.Contracts.Rag;
using SiloAI.Domains;
using System.Globalization;
using System.Text;

namespace SiloAI.Agent.Rag;

public class RagIndexingService(
    AiApiContext context,
    ITextExtractionDispatcher extractor,
    ITextChunkingService chunker,
    IEmbeddingService embeddings,
    ILogger<RagIndexingService> logger) : IRagIndexingService
{
    public async Task<RagIndexingResult> IndexAsync(Guid documentId, Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var document = await context.RagDocuments
            .FirstOrDefaultAsync(d => d.Id == documentId, cancellationToken)
            ?? throw new InvalidOperationException($"RagDocument {documentId} not found.");

        return await RunPipelineAsync(document, content, fileName, contentType, deleteExisting: false, cancellationToken);
    }

    public async Task<RagIndexingResult> RebuildAsync(Guid documentId, Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var document = await context.RagDocuments
            .FirstOrDefaultAsync(d => d.Id == documentId, cancellationToken)
            ?? throw new InvalidOperationException($"RagDocument {documentId} not found.");

        return await RunPipelineAsync(document, content, fileName, contentType, deleteExisting: true, cancellationToken);
    }

    private async Task<RagIndexingResult> RunPipelineAsync(
        RagDocument document,
        Stream content,
        string fileName,
        string contentType,
        bool deleteExisting,
        CancellationToken cancellationToken)
    {
        try
        {
            document.ProcessingStatus = RagProcessingStatus.Processing;
            document.ProcessingError = null;
            document.LastUpdateDateTime = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);

            if (deleteExisting)
            {
                await context.RagDocumentChunks
                    .Where(c => c.DocumentId == document.Id)
                    .ExecuteDeleteAsync(cancellationToken);
            }

            var text = await extractor.ExtractTextAsync(content, fileName, contentType, cancellationToken);

            var chunks = chunker.CreateChunks(text);

            if (chunks.Count == 0)
            {
                document.ProcessingStatus = RagProcessingStatus.Completed;
                document.ChunkCount = 0;
                document.LastUpdateDateTime = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);
                return new RagIndexingResult(document.Id, 0, document.ProcessingStatus);
            }

            var vectors = await embeddings.GenerateEmbeddingsAsync(
                chunks.Select(c => c.Content).ToList(),
                cancellationToken);

            if (vectors.Count != chunks.Count)
            {
                throw new InvalidOperationException(
                    $"Embedding count mismatch (expected {chunks.Count}, got {vectors.Count}).");
            }

            var now = DateTime.UtcNow;
            var entities = chunks.Select(c => new RagDocumentChunk
            {
                Id = Guid.NewGuid(),
                DocumentId = document.Id,
                ChunkIndex = c.Index,
                Content = c.Content,
                TokenCount = c.TokenCount,
                CreateDateTime = now
            }).ToList();

            context.RagDocumentChunks.AddRange(entities);
            await context.SaveChangesAsync(cancellationToken);

            // Persist embeddings into the SQL Server 2025 VECTOR column via raw SQL. EF Core 9
            // does not model VECTOR yet, so we update one row at a time, casting a JSON-array
            // literal to VECTOR(N).
            var dimensions = embeddings.Dimensions;
            for (var i = 0; i < entities.Count; i++)
            {
                var vectorLiteral = FormatVectorLiteral(vectors[i]);
                await context.Database.ExecuteSqlRawAsync(
                    "UPDATE [tbl_RagDocumentChunks] SET [fld_Embedding] = CAST({0} AS VECTOR(" + dimensions + ")) WHERE [fld_Id] = {1};",
                    [vectorLiteral, entities[i].Id],
                    cancellationToken);
            }

            document.ProcessingStatus = RagProcessingStatus.Completed;
            document.ChunkCount = entities.Count;
            document.LastUpdateDateTime = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);

            return new RagIndexingResult(document.Id, entities.Count, document.ProcessingStatus);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "RAG indexing failed for document {DocumentId}", document.Id);

            document.ProcessingStatus = RagProcessingStatus.Failed;
            document.ProcessingError = Truncate(ex.Message, 2000);
            document.LastUpdateDateTime = DateTime.UtcNow;
            await context.SaveChangesAsync(CancellationToken.None);

            return new RagIndexingResult(document.Id, document.ChunkCount, document.ProcessingStatus, ex.Message);
        }
    }

    /// <summary>
    /// Formats a float vector as the JSON-array string SQL Server's <c>VECTOR</c> type
    /// understands, e.g. <c>[0.123,-0.456,...]</c>. Uses invariant culture to guarantee
    /// a dot decimal separator.
    /// </summary>
    public static string FormatVectorLiteral(float[] vector)
    {
        var sb = new StringBuilder(vector.Length * 12 + 2);
        sb.Append('[');
        for (var i = 0; i < vector.Length; i++)
        {
            if (i > 0) sb.Append(',');
            sb.Append(vector[i].ToString("R", CultureInfo.InvariantCulture));
        }
        sb.Append(']');
        return sb.ToString();
    }

    private static string Truncate(string value, int max) =>
        string.IsNullOrEmpty(value) || value.Length <= max ? value : value[..max];
}
