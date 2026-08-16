namespace SiloAI.Application.Shared.Features;

public class RagUploadResponseDto
{
    public Guid DocumentId { get; set; }
    public int ChunkCount { get; set; }
    public string ProcessingStatus { get; set; }
    public string? ProcessingError { get; set; }
}
