using Microsoft.Extensions.Options;
using SiloAI.Agent.Chat;
using SiloAI.Agent.Rag;

namespace SiloAI.Application.Api.Features;

public class OcrCommandHandler(
    ChatAgentService agentService,
    AiApiContext dbContext,
    IOptions<OpenAIOptions> openAiOptions) : IRequestHandler<OcrCommand, OcrResponse>
{
    public async Task<OcrResponse> Handle(OcrCommand request, CancellationToken cancellationToken)
    {
        if (!await HasCreditAsync(request.CustomerId, cancellationToken))
            throw new InsufficientCreditException();

        await agentService.InitChatAgent(modelName: openAiOptions.Value.VoiceModel);

        var extractedText = await agentService.SendImageAndGetTextAsync(
            request.ImageData, request.MediaType, request.PromptKey);

        return new OcrResponse { ExtractedText = extractedText };
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
