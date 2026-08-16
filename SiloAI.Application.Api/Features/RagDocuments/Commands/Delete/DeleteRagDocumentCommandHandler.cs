namespace SiloAI.Application.Api.Features;

public class DeleteRagDocumentCommandHandler(AiApiContext context) : IRequestHandler<DeleteRagDocumentCommand, bool>
{
    public async Task<bool> Handle(DeleteRagDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await context.RagDocuments
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (document is null)
            return false;

        context.RagDocuments.Remove(document);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
