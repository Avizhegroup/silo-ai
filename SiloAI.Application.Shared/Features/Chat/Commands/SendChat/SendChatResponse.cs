namespace SiloAI.Application.Shared.Features;

public class SendChatResponse
{
    public string ResponseText { get; set; }
    public Guid ConversationId { get; set; }
    public ChatTokenUsageDto? TokenUsage { get; set; }
    public decimal PriceUsage { get; set; }
}
