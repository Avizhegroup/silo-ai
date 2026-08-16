using System.ComponentModel.DataAnnotations;

namespace SiloAI.Api.Dtos;
public class CreateApiKeyRequest
{
    [Required]
    public string Label { get; set; }

    [Required]
    public DateTime ExpiresAt { get; set; }

    public int? CustomerId { get; set; }
}
