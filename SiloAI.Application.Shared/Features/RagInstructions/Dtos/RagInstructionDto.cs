namespace SiloAI.Application.Shared.Features;

public class RagInstructionDto
{
    public Guid Id { get; set; }
    public RagDocType DocType { get; set; }
    public string? Key { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime? LastUpdateDateTime { get; set; }
}
