namespace SiloAI.Application.Api.Features;

public class DeleteCustomerCommandHandler(AiApiContext context) : IRequestHandler<DeleteCustomerCommand, bool>
{
    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await context.Customers.FindAsync([request.Id], cancellationToken);

        if (customer is null)
            return false;

        context.Customers.Remove(customer);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
