namespace SiloAI.Application.Shared.Features;

public class RagChatNewSessionCommand : IRequest<RagChatResponse>
{
    public string SystemPrompt { get; set; }
    public string? RagModel { get; set; }
    public string OwnerId { get; set; }
}
