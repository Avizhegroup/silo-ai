namespace SiloAI.Api.Dtos;

public class NewSessionRequest
{
    public List<string> PromptKeys { get; set; } = new();
}
