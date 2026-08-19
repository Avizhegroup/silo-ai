using SiloAI.Agent.Chat;

namespace SiloAI.Application.Api.Features;

public class RagChatNewSessionCommandHandler(
    ChatAgentService agentService,
    AiApiContext dbContext) : IRequestHandler<RagChatNewSessionCommand, RagChatResponse>
{
    public async Task<RagChatResponse> Handle(RagChatNewSessionCommand request, CancellationToken cancellationToken)
    {
        agentService.InitChatAgentWithInstructions(request.SystemPrompt, request.RagModel);

        var session = await agentService.CreateNewSessionAsync();
        var sessionJson = await agentService.SerializeSessionAsync(session);

        var now = DateTime.UtcNow;
        var chatSession = new AiChatSession
        {
            Id = Guid.NewGuid(),
            OwnerKey = ChatSessionOwnerKey.ForOwnerId(request.OwnerId),
            ChatType = "Rag",
            SessionState = sessionJson,
            CreatedAt = now,
            UpdatedAt = now
        };

        dbContext.AiChatSessions.Add(chatSession);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RagChatResponse
        {
            ResponseText = string.Empty,
            ConversationId = chatSession.Id,
            Citations = new()
        };
    }
}
