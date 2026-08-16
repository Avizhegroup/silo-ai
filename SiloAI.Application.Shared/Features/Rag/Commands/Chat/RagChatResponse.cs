namespace SiloAI.Application.Shared.Features;

public class RagChatResponse
{
    public string ResponseText { get; set; }
    public string UpdatedSessionJson { get; set; }
    public List<RagChatCitationDto> Citations { get; set; } = new();
    public ChatTokenUsageDto? TokenUsage { get; set; }
}
