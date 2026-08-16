using Microsoft.Extensions.Options;
using SiloAI.Application.Shared.Contracts.Rag;
using System.Text;
using System.Text.RegularExpressions;

namespace SiloAI.Agent.Rag;

/// <summary>
/// Token-aware semantic chunker. Splits text on paragraph / sentence boundaries and packs
/// the resulting fragments into chunks of <see cref="RagOptions.ChunkSize"/> approximate
/// tokens with <see cref="RagOptions.ChunkOverlap"/> token overlap.
///
/// <para>
/// "Token" here is approximated as <c>ceil(chars / 4)</c>, which is a well-known heuristic
/// for English/Latin script and matches OpenAI's token guidance closely enough for chunking
/// purposes without taking a dependency on a heavyweight tokenizer.
/// </para>
/// </summary>
public class TextChunkingService(IOptions<RagOptions> options) : ITextChunkingService
{
    private static readonly Regex ParagraphSplitter = new(@"\r?\n\s*\r?\n", RegexOptions.Compiled);
    private static readonly Regex SentenceSplitter = new(@"(?<=[\.!\?؟])\s+", RegexOptions.Compiled);

    private readonly RagOptions _options = options.Value;

    public IReadOnlyList<TextChunk> CreateChunks(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        var chunkSize = Math.Max(50, _options.ChunkSize);
        var overlap = Math.Clamp(_options.ChunkOverlap, 0, chunkSize - 1);

        var fragments = SplitIntoFragments(text);

        var chunks = new List<TextChunk>();
        var current = new StringBuilder();
        var currentTokens = 0;
        var carryOver = new List<(string Text, int Tokens)>();

        foreach (var fragment in fragments)
        {
            var fragTokens = EstimateTokens(fragment);

            // Fragment alone exceeds target — hard-split on whitespace to keep chunks bounded.
            if (fragTokens > chunkSize)
            {
                // Only flush if there is accumulated content; avoids creating an empty leading chunk.
                if (current.Length > 0)
                    FlushCurrent(chunks, current, ref currentTokens, carryOver);

                foreach (var piece in HardSplit(fragment, chunkSize))
                {
                    var trimmedPiece = piece.Trim();
                    if (trimmedPiece.Length == 0) continue;
                    chunks.Add(new TextChunk(chunks.Count, trimmedPiece, EstimateTokens(trimmedPiece)));
                }
                continue;
            }

            if (currentTokens + fragTokens > chunkSize && current.Length > 0)
            {
                FlushCurrent(chunks, current, ref currentTokens, carryOver, overlap);
            }

            if (current.Length > 0) current.Append(' ');
            current.Append(fragment);
            currentTokens += fragTokens;
            carryOver.Add((fragment, fragTokens));
        }

        var remaining = current.ToString().Trim();
        if (remaining.Length > 0)
        {
            chunks.Add(new TextChunk(chunks.Count, remaining, currentTokens));
        }

        return chunks;
    }

    private static IEnumerable<string> SplitIntoFragments(string text)
    {
        foreach (var paragraph in ParagraphSplitter.Split(text))
        {
            var trimmed = paragraph.Trim();
            if (trimmed.Length == 0) continue;

            var sentences = SentenceSplitter.Split(trimmed);
            foreach (var sentence in sentences)
            {
                var s = sentence.Trim();
                if (s.Length > 0) yield return s;
            }
        }
    }

    private static IEnumerable<string> HardSplit(string fragment, int chunkSize)
    {
        var maxChars = chunkSize * 4;
        for (var i = 0; i < fragment.Length; i += maxChars)
        {
            yield return fragment.Substring(i, Math.Min(maxChars, fragment.Length - i));
        }
    }

    private static void FlushCurrent(
        List<TextChunk> chunks,
        StringBuilder current,
        ref int currentTokens,
        List<(string Text, int Tokens)> carryOver,
        int overlap = 0)
    {
        var content = current.ToString().Trim();
        if (content.Length > 0)
        {
            chunks.Add(new TextChunk(chunks.Count, content, currentTokens));
        }

        current.Clear();
        currentTokens = 0;

        if (overlap <= 0)
        {
            carryOver.Clear();
            return;
        }

        // Re-seed the next chunk with the trailing fragments worth ~overlap tokens.
        var tail = new List<(string Text, int Tokens)>();
        var tailTokens = 0;
        for (var i = carryOver.Count - 1; i >= 0 && tailTokens < overlap; i--)
        {
            tail.Insert(0, carryOver[i]);
            tailTokens += carryOver[i].Tokens;
        }

        carryOver.Clear();
        foreach (var (txt, tokens) in tail)
        {
            if (current.Length > 0) current.Append(' ');
            current.Append(txt);
            currentTokens += tokens;
            carryOver.Add((txt, tokens));
        }
    }

    /// <summary>
    /// Approximate OpenAI token count by dividing the visible character length by 4.
    /// Good enough for chunk-budgeting without bringing in a tokenizer dependency.
    /// </summary>
    public static int EstimateTokens(string text) =>
        string.IsNullOrEmpty(text) ? 0 : (int)Math.Ceiling(text.Length / 4d);
}
