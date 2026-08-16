namespace SiloAI.Application.Shared.Contracts.Rag;

public record TextChunk(int Index, string Content, int TokenCount);

/// <summary>
/// Splits a raw text document into semantic chunks suitable for embedding. Implementations
/// preserve paragraph boundaries and avoid splitting sentences mid-word.
/// </summary>
public interface ITextChunkingService
{
    IReadOnlyList<TextChunk> CreateChunks(string text);
}
