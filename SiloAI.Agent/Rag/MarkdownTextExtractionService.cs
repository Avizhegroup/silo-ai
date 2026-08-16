using Markdig;
using SiloAI.Application.Shared.Contracts.Rag;
using System.Text;

namespace SiloAI.Agent.Rag;

/// <summary>
/// Extracts the plain-text content of a Markdown document by rendering it through Markdig and
/// stripping the resulting HTML tags. This keeps headings/lists/paragraph breaks intact while
/// dropping inline markup.
/// </summary>
public class MarkdownTextExtractionService : ITextExtractionService
{
    public IReadOnlyCollection<string> SupportedExtensions { get; } = [".md", ".markdown"];

    public async Task<string> ExtractTextAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(content, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
        var markdown = await reader.ReadToEndAsync(cancellationToken);

        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var html = Markdown.ToHtml(markdown, pipeline);

        return StripHtml(html);
    }

    private static string StripHtml(string html)
    {
        var sb = new StringBuilder(html.Length);
        var inTag = false;
        foreach (var ch in html)
        {
            if (ch == '<')
            {
                inTag = true;
                continue;
            }
            if (ch == '>')
            {
                inTag = false;
                sb.Append('\n');
                continue;
            }
            if (!inTag)
            {
                sb.Append(ch);
            }
        }
        return System.Net.WebUtility.HtmlDecode(sb.ToString());
    }
}
