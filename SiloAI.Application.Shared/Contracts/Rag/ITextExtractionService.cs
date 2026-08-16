namespace SiloAI.Application.Shared.Contracts.Rag;

/// <summary>
/// Extracts plain text from an uploaded knowledge file. Implementations are picked by
/// <c>contentType</c> / file extension.
/// </summary>
public interface ITextExtractionService
{
    /// <summary>Returns the file extensions this extractor handles (lower-case, includes dot).</summary>
    IReadOnlyCollection<string> SupportedExtensions { get; }

    Task<string> ExtractTextAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken);
}

/// <summary>
/// Aggregates registered <see cref="ITextExtractionService"/> implementations and dispatches
/// to the correct one based on file extension.
/// </summary>
public interface ITextExtractionDispatcher
{
    bool IsSupported(string fileName);
    Task<string> ExtractTextAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken);
}
