namespace SiloAI.Application.Api.Features;

public class CreateCustomerCommandHandler(AiApiContext context) : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Name = request.Name,
            RemainingCredit = request.RemainingCredit,
            CreatedAt = DateTime.UtcNow
        };

        context.Customers.Add(customer);
        await context.SaveChangesAsync(cancellationToken);

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            RemainingCredit = customer.RemainingCredit,
            CreatedAt = customer.CreatedAt
        };
    }
}
