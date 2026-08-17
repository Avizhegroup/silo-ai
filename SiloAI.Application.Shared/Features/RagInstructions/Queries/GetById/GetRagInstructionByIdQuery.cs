namespace SiloAI.Application.Shared.Features;

public class GetRagInstructionByIdQuery : IRequest<RagInstructionDto?>
{
    public Guid Id { get; set; }
}
