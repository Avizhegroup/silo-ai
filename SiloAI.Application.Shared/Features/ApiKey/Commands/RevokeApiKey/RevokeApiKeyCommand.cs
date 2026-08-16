namespace SiloAI.Application.Shared.Features;

public class RevokeApiKeyCommand : IRequest<bool>
{
    public int Id { get; set; }
}
