namespace SiloAI.Application.Api.Features;

public class GetCustomerApiKeysQueryHandler(AiApiContext context) : IRequestHandler<GetCustomerApiKeysQuery, List<ApiKeyDto>>
{
    public async Task<List<ApiKeyDto>> Handle(GetCustomerApiKeysQuery request, CancellationToken cancellationToken)
    {
        return await context.AiApiKeys
            .AsNoTracking()
            .Where(k => k.CustomerId == request.CustomerId)
            .OrderByDescending(k => k.CreatedAt)
            .Select(k => new ApiKeyDto
            {
                Id = k.Id,
                Label = k.Label,
                ExpiresAt = k.ExpiresAt,
                IsRevoked = k.IsRevoked,
                CreatedAt = k.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
