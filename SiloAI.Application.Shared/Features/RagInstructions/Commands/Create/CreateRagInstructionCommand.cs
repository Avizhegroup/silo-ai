namespace SiloAI.Application.Shared.Features;

public class CreateRagInstructionCommand : IRequest<RagInstructionDto>
{
    public RagDocType? DocType { get; set; }
    public string? Key { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public string Content { get; set; }
    public string? CreatorUserId { get; set; }
}
