namespace SiloAI.Application.Shared.Features;

public class NewChatSessionCommands : IRequest<NewChatSessionVm>
{
    public string UserId { get; set; }
    public ChatPageMode Mode { get; set; }
    public List<string> PromptKeys { get; set; } = new();
}
