namespace SiloAI.Application.Shared.Features;

public class RagDocumentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string OriginalFileName { get; set; }
    public string ContentType { get; set; }
    public RagDocType DocType { get; set; }
    public string? Key { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public string FileHash { get; set; }
    public long FileSize { get; set; }
    public string ProcessingStatus { get; set; }
    public string? ProcessingError { get; set; }
    public int ChunkCount { get; set; }
    public DateTime CreateDateTime { get; set; }
    public string? CreatorUserId { get; set; }
    public DateTime? LastUpdateDateTime { get; set; }
}
