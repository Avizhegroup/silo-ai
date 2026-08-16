namespace SiloAI.Application.Shared.Contracts.Rag;

/// <summary>
/// Generates dense vector embeddings for arbitrary text using a remote provider.
/// </summary>
public interface IEmbeddingService
{
    /// <summary>Returns the configured embedding model name (for diagnostics / column sizing).</summary>
    string ModelName { get; }

    /// <summary>Returns the dimensionality of vectors produced by <see cref="ModelName"/>.</summary>
    int Dimensions { get; }

    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken);

    Task<IReadOnlyList<float[]>> GenerateEmbeddingsAsync(IReadOnlyList<string> texts, CancellationToken cancellationToken);
}
