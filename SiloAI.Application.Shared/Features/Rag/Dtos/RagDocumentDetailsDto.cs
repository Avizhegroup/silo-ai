namespace SiloAI.Application.Shared.Features;

public class RagDocumentDetailsDto : RagDocumentDto
{
    public List<RagDocumentChunkDto> Chunks { get; set; } = new();
}
