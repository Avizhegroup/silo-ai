namespace SiloAI.Application.Api.Features;

public class GetAllApiKeysQueryHandler(AiApiContext context) : IRequestHandler<GetAllApiKeysQuery, List<ApiKeyDto>>
{
    public async Task<List<ApiKeyDto>> Handle(GetAllApiKeysQuery request, CancellationToken cancellationToken)
    {
        return await context.AiApiKeys
            .AsNoTracking()
            .OrderByDescending(k => k.CreatedAt)
            .Select(k => new ApiKeyDto
            {
                Id = k.Id,
                Label = k.Label,
                ExpiresAt = k.ExpiresAt,
                IsRevoked = k.IsRevoked,
                CreatedAt = k.CreatedAt,
                CustomerId = k.CustomerId
            })
            .ToListAsync(cancellationToken);
    }
}
