using Microsoft.EntityFrameworkCore;
using SiloAI.Application.Shared.Contracts.Rag;
using SiloAI.Domains;
using System.Data;

namespace SiloAI.Agent.Rag;

public class RagSearchService(
    AiApiContext context,
    IEmbeddingService embeddings) : IRagSearchService
{
    public async Task<IReadOnlyList<RagSearchHit>> SearchAsync(
        string query,
        int topK,
        string? docType,
        string? key,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query)) return [];

        var top = Math.Clamp(topK, 1, 100);

        var queryVector = await embeddings.GenerateEmbeddingAsync(query, cancellationToken);
        var literal = RagIndexingService.FormatVectorLiteral(queryVector);
        var dims = embeddings.Dimensions;

        // Build optional WHERE predicates for DocType and Key filters.
        var docTypeFilter = string.IsNullOrWhiteSpace(docType) ? string.Empty
            : "AND d.[fld_DocType] = @docType\n";
        var keyFilter = string.IsNullOrWhiteSpace(key) ? string.Empty
            : "AND d.[fld_Key] = @key\n";

        // TOP cannot be parameterised. The value is clamped above so it cannot exceed the
        // validated range, making the inlined value safe.
        var sql = $@"
SELECT TOP({top})
    c.[fld_Id]               AS ChunkId,
    c.[fld_DocumentId]       AS DocumentId,
    d.[fld_OriginalFileName] AS FileName,
    d.[fld_Category]         AS Category,
    c.[fld_ChunkIndex]       AS ChunkIndex,
    c.[fld_Content]          AS Content,
    VECTOR_DISTANCE('cosine', c.[fld_Embedding], CAST(@queryVector AS VECTOR({dims}))) AS Distance
FROM [tbl_RagDocumentChunks] c
INNER JOIN [tbl_RagDocuments] d ON d.[fld_Id] = c.[fld_DocumentId]
WHERE c.[fld_Embedding] IS NOT NULL
{docTypeFilter}{keyFilter}ORDER BY Distance ASC;";

        var connection = context.Database.GetDbConnection();
        var openedHere = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
            openedHere = true;
        }

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = sql;

            var vectorParam = command.CreateParameter();
            vectorParam.ParameterName = "@queryVector";
            vectorParam.DbType = DbType.String;
            vectorParam.Value = literal;
            command.Parameters.Add(vectorParam);

            if (!string.IsNullOrWhiteSpace(docType))
            {
                var docTypeParam = command.CreateParameter();
                docTypeParam.ParameterName = "@docType";
                docTypeParam.DbType = DbType.String;
                docTypeParam.Value = docType;
                command.Parameters.Add(docTypeParam);
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                var keyParam = command.CreateParameter();
                keyParam.ParameterName = "@key";
                keyParam.DbType = DbType.String;
                keyParam.Value = key;
                command.Parameters.Add(keyParam);
            }

            var hits = new List<RagSearchHit>(top);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                var distance = reader.GetDouble(reader.GetOrdinal("Distance"));
                hits.Add(new RagSearchHit(
                    ChunkId: reader.GetGuid(reader.GetOrdinal("ChunkId")),
                    DocumentId: reader.GetGuid(reader.GetOrdinal("DocumentId")),
                    FileName: reader.GetString(reader.GetOrdinal("FileName")),
                    Category: reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                    ChunkIndex: reader.GetInt32(reader.GetOrdinal("ChunkIndex")),
                    Content: reader.GetString(reader.GetOrdinal("Content")),
                    Distance: distance,
                    Similarity: 1d - distance));
            }
            return hits;
        }
        finally
        {
            if (openedHere)
            {
                await connection.CloseAsync();
            }
        }
    }
}
