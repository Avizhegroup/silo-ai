namespace SiloAI.Application.Shared.Features;

public class DeleteCustomerCommand : IRequest<bool>
{
    public int Id { get; set; }
}
