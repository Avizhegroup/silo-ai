namespace SiloAI.Application.Shared.Features;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal RemainingCredit { get; set; }
    public DateTime CreatedAt { get; set; }
}
