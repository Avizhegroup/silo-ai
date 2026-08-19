namespace SiloAI.Application.Shared.Features;

public class RagChatSendCommand : IRequest<RagChatResponse>
{
    public Guid? ConversationId { get; set; }
    public string Message { get; set; }
    public int TopK { get; set; } = 5;
    public bool IsMainChat { get; set; }
    public RagDocType DocType { get; set; } = RagDocType.GeneralChat;
    public string? Key { get; set; }
    public string SystemPrompt { get; set; }
    public string SystemPromptMainChat { get; set; }
    public string AugmentedMessageTemplate { get; set; }
    public string? RagModel { get; set; }
    public string Username { get; set; }
    public string OwnerId { get; set; }
}
