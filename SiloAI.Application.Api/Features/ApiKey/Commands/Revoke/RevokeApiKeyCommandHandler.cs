namespace SiloAI.Application.Api.Features;

public class RevokeApiKeyCommandHandler(AiApiContext context) : IRequestHandler<RevokeApiKeyCommand, bool>
{
    public async Task<bool> Handle(RevokeApiKeyCommand request, CancellationToken cancellationToken)
    {
        var apiKey = await context.AiApiKeys.FindAsync([request.Id], cancellationToken);

        if (apiKey is null)
            return false;

        apiKey.IsRevoked = true;
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
