namespace SiloAI.Application.Shared.Features;

public class OcrCommand : IRequest<OcrResponse>
{
    public byte[] ImageData { get; set; }
    public string MediaType { get; set; }
    public string? PromptKey { get; set; }
    public int? CustomerId { get; set; }
}
