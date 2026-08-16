namespace SiloAI.Application.Api.Features;

public class GetAllCustomersQueryHandler(AiApiContext context) : IRequestHandler<GetAllCustomersQuery, List<CustomerDto>>
{
    public async Task<List<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return await context.Customers
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                RemainingCredit = c.RemainingCredit,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
