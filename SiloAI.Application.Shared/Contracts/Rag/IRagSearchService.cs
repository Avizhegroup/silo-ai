namespace SiloAI.Application.Shared.Contracts.Rag;

public record RagSearchHit(
    Guid ChunkId,
    Guid DocumentId,
    string FileName,
    string? Category,
    int ChunkIndex,
    string Content,
    double Distance,
    double Similarity);

/// <summary>
/// Semantic retrieval over the RAG knowledge base. The implementation embeds the user
/// query and ranks chunks by SQL Server 2025 <c>VECTOR_DISTANCE('cosine', …)</c>.
/// </summary>
public interface IRagSearchService
{
    /// <summary>
    /// Searches for the top-K semantically similar chunks.
    /// </summary>
    /// <param name="query">The user query to embed and search.</param>
    /// <param name="topK">Maximum number of results to return.</param>
    /// <param name="docType">When specified, restricts results to documents with this type.</param>
    /// <param name="key">When specified (and non-empty), restricts results to documents with this key.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<RagSearchHit>> SearchAsync(
        string query,
        int topK,
        string? docType,
        string? key,
        CancellationToken cancellationToken);
}
