namespace SiloAI.Application.Shared.Features;

public class SendChatCommand : IRequest<SendChatResponse>
{
    public Guid? ConversationId { get; set; }
    public string Message { get; set; }
    public string Username { get; set; }
    public List<string> PromptKeys { get; set; } = new();
    public int? CustomerId { get; set; }
}
