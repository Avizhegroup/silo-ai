using SiloAI.Application.Shared.Contracts.Rag;
using System.Text;

namespace SiloAI.Agent.Rag;

public class TxtTextExtractionService : ITextExtractionService
{
    public IReadOnlyCollection<string> SupportedExtensions { get; } = [".txt"];

    public async Task<string> ExtractTextAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(content, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
        return await reader.ReadToEndAsync(cancellationToken);
    }
}
