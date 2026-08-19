using SiloAI.Agent.Chat;
using System.Text;

namespace SiloAI.Application.Api.Features;

public class RagChatSendHandler(
    ChatAgentService agentService,
    IRagSearchService search,
    IMediator mediator,
    AiApiContext dbContext) : IRequestHandler<RagChatSendCommand, RagChatResponse>
{
    public async Task<RagChatResponse> Handle(RagChatSendCommand request, CancellationToken cancellationToken)
    {
        var ownerKey = ChatSessionOwnerKey.ForOwnerId(request.OwnerId);

        AiChatSession? chatSession = null;
        string? existingSessionJson = null;

        if (request.ConversationId.HasValue)
        {
            chatSession = await dbContext.AiChatSessions
                .FirstOrDefaultAsync(s => s.Id == request.ConversationId.Value, cancellationToken);

            if (chatSession is null || chatSession.OwnerKey != ownerKey)
                throw new ConversationNotFoundException();

            existingSessionJson = chatSession.SessionState;
        }

        var systemPrompt = request.IsMainChat ? request.SystemPromptMainChat : request.SystemPrompt;

        var instructions = await mediator.Send(new GetAllRagInstructionsQuery
        {
            DocType = request.DocType,
            IsActive = true
        }, cancellationToken);

        var agentInstructions = BuildAgentInstructions(systemPrompt, instructions);

        agentService.InitChatAgentWithInstructions(agentInstructions, request.RagModel);

        var topK = request.TopK <= 0 ? 5 : Math.Clamp(request.TopK, 1, 20);
     
        var hits = await search.SearchAsync(
            request.Message, topK, request.DocType.ToString(), request.Key, cancellationToken);

        var augmentedMessage = BuildAugmentedMessage(
            request.Message, hits, request.IsMainChat, request.AugmentedMessageTemplate);

        var query = new CopilotMessageRequest
        {
            Text = augmentedMessage,
            Username = request.Username,
            SiloChatId = Guid.NewGuid().ToString(),
            IsUser = true,
            Datetime = DateTime.Now
        };

        var (response, updatedSessionJson, tokenUsage) = await agentService.SendWithAgentSessionAsync(existingSessionJson, query);

        var now = DateTime.UtcNow;
        if (chatSession is null)
        {
            chatSession = new AiChatSession
            {
                Id = Guid.NewGuid(),
                OwnerKey = ownerKey,
                ChatType = "Rag",
                CreatedAt = now
            };
            dbContext.AiChatSessions.Add(chatSession);
        }

        chatSession.SessionState = updatedSessionJson;
        chatSession.UpdatedAt = now;

        await dbContext.SaveChangesAsync(cancellationToken);

        var citations = request.IsMainChat
            ? new List<RagChatCitationDto>()
            : hits.Select(h => new RagChatCitationDto
            {
                ChunkId = h.ChunkId,
                DocumentId = h.DocumentId,
                FileName = h.FileName,
                Category = h.Category,
                ChunkIndex = h.ChunkIndex,
                Similarity = h.Similarity,
                Snippet = Truncate(h.Content, 280)
            }).ToList();

        return new RagChatResponse
        {
            ResponseText = response.ResponseText,
            ConversationId = chatSession.Id,
            TokenUsage = tokenUsage,
            Citations = citations
        };
    }

    private static string BuildAgentInstructions(string systemPrompt, List<RagInstructionDto> instructions)
    {
        if (instructions is null || instructions.Count == 0)
            return systemPrompt;

        var docTypeInstructionsText = string.Join("\n---\n", instructions
            .OrderBy(i => i.CreateDateTime)
            .Select(i => i.Content));

        return $"{systemPrompt}\n\n{docTypeInstructionsText}";
    }

    private static string BuildAugmentedMessage(
        string userQuestion,
        IReadOnlyList<RagSearchHit> hits,
        bool isMainChat,
        string augmentedMessageTemplate)
    {
        string chunksText;

        if (hits is null || hits.Count == 0)
        {
            chunksText = "(هیچ قطعه‌ای از پایگاه دانش برای این پرسش پیدا نشد.)";
        }
        else
        {
            var sb = new StringBuilder();
            for (var i = 0; i < hits.Count; i++)
            {
                var h = hits[i];
                if (!isMainChat)
                    sb.AppendLine($"[{i + 1}] فایل: {h.FileName} | قطعه: {h.ChunkIndex} | شباهت: {h.Similarity:F3}");
                sb.AppendLine(h.Content);
                sb.AppendLine("---");
            }
            chunksText = sb.ToString().TrimEnd();
        }

        return augmentedMessageTemplate
            .Replace("{DOCTYPE_INSTRUCTIONS}", string.Empty)
            .Replace("{CHUNKS}", chunksText)
            .Replace("{QUESTION}", userQuestion);
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return value.Length <= maxLength ? value : value[..maxLength] + "…";
    }
}
