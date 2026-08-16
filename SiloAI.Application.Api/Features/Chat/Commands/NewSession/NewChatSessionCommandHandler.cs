using SiloAI.Agent.Chat;
using NewChatSessionCommand = SiloAI.Application.Shared.Features.NewChatSessionCommand;

namespace SiloAI.Application.Api.Features;

public class NewChatSessionCommandHandler(
    ChatAgentService agentService,
    AiApiContext dbContext) : IRequestHandler<NewChatSessionCommand, NewSessionResponse>
{
    public async Task<NewSessionResponse> Handle(NewChatSessionCommand request, CancellationToken cancellationToken)
    {
        if (!await HasCreditAsync(request.CustomerId, cancellationToken))
            throw new InsufficientCreditException();

        await agentService.InitChatAgent(request.PromptKeys);

        var session = await agentService.CreateNewSessionAsync();
        var sessionJson = await agentService.SerializeSessionAsync(session);

        return new NewSessionResponse { SessionJson = sessionJson };
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
