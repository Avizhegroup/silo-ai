namespace SiloAI.Application.Shared.Features;

public class UpdateRagInstructionCommand : IRequest<RagInstructionDto?>
{
    public Guid Id { get; set; }
    public RagDocType? DocType { get; set; }
    public string? Key { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
    public string? UpdaterUserId { get; set; }
}
