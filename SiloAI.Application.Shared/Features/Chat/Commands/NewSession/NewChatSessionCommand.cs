namespace SiloAI.Application.Shared.Features;

public class NewChatSessionCommand : IRequest<NewSessionResponse>
{
    public List<string> PromptKeys { get; set; } = new();
    public int? CustomerId { get; set; }
}
