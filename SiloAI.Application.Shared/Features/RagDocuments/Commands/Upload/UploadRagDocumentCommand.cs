namespace SiloAI.Application.Shared.Features;

public class UploadRagDocumentCommand : IRequest<RagUploadResponseDto>
{
    public byte[] FileContent { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public RagDocType? DocType { get; set; }
    public string? Key { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public string? CreatorUserId { get; set; }
}
