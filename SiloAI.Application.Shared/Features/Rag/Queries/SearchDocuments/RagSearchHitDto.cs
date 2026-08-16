namespace SiloAI.Application.Shared.Features;

public class RagSearchHitDto
{
    public Guid ChunkId { get; set; }
    public Guid DocumentId { get; set; }
    public string FileName { get; set; }
    public string? Category { get; set; }
    public int ChunkIndex { get; set; }
    public string Content { get; set; }
    public double Distance { get; set; }
    public double Similarity { get; set; }
}
