namespace SiloAI.Application.Shared.Features;

public class SendChatMessageVm
{
    public string ResponseText { get; set; }
    public int SessionId { get; set; }
    public List<List<object>> SqlCommandsResults { get; set; } = new();
}
