namespace SiloAI.Api.Dtos;

public class SendChatRequest
{
    public string? SessionJson { get; set; }
    public string Message { get; set; }
    public string Username { get; set; }
    public List<string> PromptKeys { get; set; } = new();
}
