using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SiloAI.Agent.Chat;

namespace SiloAI.Application.Api.Features;

public class SendChatCommandHandler(
    ChatAgentService agentService,
    AiApiContext dbContext,
    IServiceScopeFactory scopeFactory,
    ILogger<SendChatCommandHandler> logger,
    AiCostCalculator costCalculator)
    : IRequestHandler<SendChatCommand, SendChatResponse>
{
    public async Task<SendChatResponse> Handle(SendChatCommand request, CancellationToken cancellationToken)
    {
        if (!await HasCreditAsync(request.CustomerId, cancellationToken))
            throw new InsufficientCreditException();

        var ownerKey = ChatSessionOwnerKey.ForCustomer(request.CustomerId);

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

        await agentService.InitChatAgent(request.PromptKeys);

        var query = new CopilotMessageRequest
        {
            Text = request.Message,
            Username = request.Username,
            SiloChatId = Guid.NewGuid().ToString(),
            IsUser = true,
            Datetime = DateTime.Now
        };

        var ( response, updatedSessionJson, tokenUsage) = await agentService.SendWithAgentSessionAsync(existingSessionJson, query);
        var priceUsage = costCalculator.Calculate(tokenUsage);

        var customer = await dbContext.Customers.FirstOrDefaultAsync( c => c.Id == request.CustomerId,cancellationToken);
        if (customer is not null)
        {
            customer.RemainingCredit -= priceUsage;

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var now = DateTime.UtcNow;
        if (chatSession is null)
        {
            chatSession = new AiChatSession
            {
                Id = Guid.NewGuid(),
                OwnerKey = ownerKey,
                ChatType = "Chat",
                CreatedAt = now
            };
            dbContext.AiChatSessions.Add(chatSession);
        }

        chatSession.SessionState = updatedSessionJson;
        chatSession.UpdatedAt = now;

        await dbContext.SaveChangesAsync(cancellationToken);

        var instructionKey = request.PromptKeys?.FirstOrDefault();
        var userAsk = request.Message;
        var botAnswer = response.ResponseText;
        var customerId = request.CustomerId;

        _ = Task.Run(async () =>
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AiApiContext>();
            try
            {
                db.AiConversations.Add(new AiConversation
                {
                    UserAsk = userAsk ?? string.Empty,
                    BotAnswer = botAnswer ?? string.Empty,
                    InstructionKey = instructionKey,
                    CreditUsage = null,
                    LocalConversationId = 0,
                    CustomerId = customerId ?? 0,
                    CreatedAt = DateTime.UtcNow
                });
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to save AI conversation to database.");
            }
        });
        
        return new SendChatResponse
        {
            ResponseText = response.ResponseText,
            ConversationId = chatSession.Id,
            TokenUsage = tokenUsage,
            PriceUsage = priceUsage
        };
    }

    private async Task<bool> HasCreditAsync(int? customerId, CancellationToken cancellationToken)
    {
        if (customerId is null) return true;

        var customer = await dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == customerId.Value, cancellationToken);

        return customer is null || customer.RemainingCredit > 0;
    }
}
