namespace SiloAI.Application.Shared.Features;

public class RagDocumentChunkDto
{
    public Guid Id { get; set; }
    public int ChunkIndex { get; set; }
    public string Content { get; set; }
    public int TokenCount { get; set; }
    public DateTime CreateDateTime { get; set; }
}
