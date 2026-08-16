namespace SiloAI.Application.Shared.Features;

public class CopilotMessageRequest
{
    public string Username { get; set; }
    public string Text { get; set; }
    public string SiloChatId { get; set; }
    public bool IsUser { get; set; }
    public DateTime Datetime { get; set; }
    public List<string> SqlCommands { get; set; } = new();
    public List<List<object>> SqlCommandsResults { get; set; } = new();
}
