namespace SiloAI.Application.Shared.Features;

public class GetRagDocumentByIdQuery : IRequest<RagDocumentDetailsDto?>
{
    public Guid Id { get; set; }
}
