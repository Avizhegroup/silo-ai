namespace SiloAI.Application.Shared.Features;
public class DeleteChatSessionsCommand : IRequest<DeleteChatSessionsVm>
{
    public int SessionId { get; set; }
    public string UserId { get; set; }
}
