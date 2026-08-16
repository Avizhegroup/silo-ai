namespace SiloAI.Application.Shared.Contracts.Rag;

public record RagIndexingResult(Guid DocumentId, int ChunkCount, string Status, string? Error = null);

/// <summary>
/// End-to-end RAG indexing pipeline: extract text → chunk → embed → persist.
/// </summary>
public interface IRagIndexingService
{
    /// <summary>
    /// Indexes the supplied document. The document row is expected to already exist
    /// (typically created by the upload command) — this call updates its status,
    /// chunk count, and persists the generated chunks + embeddings.
    /// </summary>
    Task<RagIndexingResult> IndexAsync(Guid documentId, Stream content, string fileName, string contentType, CancellationToken cancellationToken);

    /// <summary>Drops the existing chunks for a document and regenerates embeddings from the original text.</summary>
    Task<RagIndexingResult> RebuildAsync(Guid documentId, Stream content, string fileName, string contentType, CancellationToken cancellationToken);
}
