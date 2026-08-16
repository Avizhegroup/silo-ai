using SiloAI.Agent.Chat;

namespace SiloAI.Application.Api.Features;

public class RagChatNewSessionCommandHandler(
    ChatAgentService agentService) : IRequestHandler<RagChatNewSessionCommand, RagChatResponse>
{
    public async Task<RagChatResponse> Handle(RagChatNewSessionCommand request, CancellationToken cancellationToken)
    {
        agentService.InitChatAgentWithInstructions(request.SystemPrompt, request.RagModel);

        var session = await agentService.CreateNewSessionAsync();
        var sessionJson = await agentService.SerializeSessionAsync(session);

        return new RagChatResponse
        {
            ResponseText = string.Empty,
            UpdatedSessionJson = sessionJson,
            Citations = new()
        };
    }
}
