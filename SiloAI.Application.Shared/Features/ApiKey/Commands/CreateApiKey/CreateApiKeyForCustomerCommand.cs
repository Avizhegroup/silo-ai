namespace SiloAI.Application.Shared.Features;

public class CreateApiKeyForCustomerCommand : IRequest<ApiKeyDto>
{
    public string Label { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int? CustomerId { get; set; }
}
