namespace SiloAI.Application.Shared.Features;

public class CreateCustomerCommand : IRequest<CustomerDto>
{
    public string Name { get; set; }
    public decimal RemainingCredit { get; set; }
}
