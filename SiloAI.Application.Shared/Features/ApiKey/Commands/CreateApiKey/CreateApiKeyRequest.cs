namespace SiloAI.Application.Shared.Features;

public class CreateApiKeyRequest
{
    [Required]
    public string Label { get; set; }

    [Required]
    public DateTime ExpiresAt { get; set; }

    public int? CustomerId { get; set; }
}
