namespace SiloAI.Application.Shared.Features;

public class GetChatHistoriesQuery : IRequest<GetChatHistoriesVm>
{
    public string UserId { get; set; }
    public ChatPageMode? Mode { get; set; }
}
