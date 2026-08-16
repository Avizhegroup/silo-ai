namespace SiloAI.Application.Shared.Features;

public class SendChatMessageCommand : IRequest<SendChatMessageVm>
{
    public string UserId { get; set; }
    public int SessionId { get; set; }
    public string Message { get; set; }
    public ChatPageMode Mode { get; set; }
    public List<string> PromptKeys { get; set; } = new();
}
