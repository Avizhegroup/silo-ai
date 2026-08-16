namespace SiloAI.Application.Shared.Features;

public class RebuildRagDocumentCommand : IRequest<RagUploadResponseDto>
{
    public Guid Id { get; set; }
    public byte[] FileContent { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string? UpdaterUserId { get; set; }
}
