namespace SiloAI.Application.Shared.Features;

public class ChatHistory
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<CopilotMessageRequest> Messages { get; set; }
    public string? AgentSessionJson { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdated { get; set; }
}

