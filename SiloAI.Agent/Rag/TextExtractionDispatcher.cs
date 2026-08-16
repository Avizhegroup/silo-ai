using SiloAI.Application.Shared.Contracts.Rag;

namespace SiloAI.Agent.Rag;

public class TextExtractionDispatcher(IEnumerable<ITextExtractionService> extractors) : ITextExtractionDispatcher
{
    private readonly Dictionary<string, ITextExtractionService> _byExtension =
        extractors
            .SelectMany(e => e.SupportedExtensions.Select(ext => (ext: ext.ToLowerInvariant(), service: e)))
            .GroupBy(x => x.ext)
            .ToDictionary(g => g.Key, g => g.First().service);

    public bool IsSupported(string fileName) =>
        _byExtension.ContainsKey((Path.GetExtension(fileName) ?? string.Empty).ToLowerInvariant());

    public Task<string> ExtractTextAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var ext = (Path.GetExtension(fileName) ?? string.Empty).ToLowerInvariant();
        if (!_byExtension.TryGetValue(ext, out var extractor))
        {
            throw new NotSupportedException($"No text extractor is registered for extension '{ext}'.");
        }
        return extractor.ExtractTextAsync(content, fileName, contentType, cancellationToken);
    }
}
