namespace SiloAI.Application.Shared.Features;

public class DeleteRagDocumentCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
