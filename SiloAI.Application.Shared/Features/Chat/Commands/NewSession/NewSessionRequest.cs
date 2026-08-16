namespace SiloAI.Application.Shared.Features;

public class NewSessionRequest
{
    public List<string> PromptKeys { get; set; } = new();
}
