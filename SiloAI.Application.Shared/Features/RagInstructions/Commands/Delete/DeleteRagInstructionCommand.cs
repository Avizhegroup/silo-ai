namespace SiloAI.Application.Shared.Features;

public class DeleteRagInstructionCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
