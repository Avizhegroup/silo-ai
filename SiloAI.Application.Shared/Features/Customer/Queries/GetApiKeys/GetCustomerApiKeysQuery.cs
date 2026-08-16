namespace SiloAI.Application.Shared.Features;

public class GetCustomerApiKeysQuery : IRequest<List<ApiKeyDto>>
{
    public int CustomerId { get; set; }
}
