namespace SiloAI.Application.Shared.Features;

public class SendChatRequest
{
    public Guid? ConversationId { get; set; }
    public string Message { get; set; }
    public string Username { get; set; }
    public List<string> PromptKeys { get; set; } = new();
}
