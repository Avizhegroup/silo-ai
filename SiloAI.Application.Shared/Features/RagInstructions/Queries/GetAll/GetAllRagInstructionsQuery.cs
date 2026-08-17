namespace SiloAI.Application.Shared.Features;

public class GetAllRagInstructionsQuery : IRequest<List<RagInstructionDto>>
{
    public RagDocType? DocType { get; set; }
    public bool? IsActive { get; set; }
}
