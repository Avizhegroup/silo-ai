namespace SiloAI.Application.Shared.Features;

public class ApiKeyDto
{
    public int Id { get; set; }
    public string Label { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? KeyValue { get; set; }
    public int? CustomerId { get; set; }
}
