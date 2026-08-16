namespace SiloAI.Application.Shared.Features;

public class GetCustomerByIdQuery : IRequest<CustomerDto?>
{
    public int Id { get; set; }
}
