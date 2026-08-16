namespace SiloAI.UI.Dtos;

public class CreateApiKeyRequest
{
    public string Label { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int? CustomerId { get; set; }
}
