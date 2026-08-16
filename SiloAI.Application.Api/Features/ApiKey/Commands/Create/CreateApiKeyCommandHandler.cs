using System.Security.Cryptography;
using System.Text;

namespace SiloAI.Application.Api.Features;

public class CreateApiKeyCommandHandler(AiApiContext context) : IRequestHandler<CreateApiKeyForCustomerCommand, ApiKeyDto>
{
    public async Task<ApiKeyDto> Handle(CreateApiKeyForCustomerCommand request, CancellationToken cancellationToken)
    {
        var rawBytes = new byte[32];
        RandomNumberGenerator.Fill(rawBytes);
        var plainKey = "sai_" + Convert.ToBase64String(rawBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

        var keyHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(plainKey))).ToLowerInvariant();

        var apiKey = new AiApiKey
        {
            KeyValue = keyHash,
            Label = request.Label,
            ExpiresAt = request.ExpiresAt,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
            CustomerId = request.CustomerId
        };

        context.AiApiKeys.Add(apiKey);
        await context.SaveChangesAsync(cancellationToken);

        return new ApiKeyDto
        {
            Id = apiKey.Id,
            Label = apiKey.Label,
            ExpiresAt = apiKey.ExpiresAt,
            IsRevoked = apiKey.IsRevoked,
            CreatedAt = apiKey.CreatedAt,
            KeyValue = plainKey,
            CustomerId = apiKey.CustomerId
        };
    }
}
