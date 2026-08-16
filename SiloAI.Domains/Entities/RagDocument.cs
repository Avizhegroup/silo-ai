namespace SiloAI.Domains;

[Table("tbl_RagDocuments")]
public class RagDocument
{
    [Column("fld_Id")]
    public Guid Id { get; set; }

    [Required]
    [Column("fld_FileName")]
    [StringLength(260)]
    public string FileName { get; set; }

    [Required]
    [Column("fld_OriginalFileName")]
    [StringLength(260)]
    public string OriginalFileName { get; set; }

    [Required]
    [Column("fld_ContentType")]
    [StringLength(150)]
    public string ContentType { get; set; }

    [Required]
    [Column("fld_DocType")]
    [StringLength(50)]
    public string DocType { get; set; } = "GeneralChat";

    [Column("fld_Key")]
    [StringLength(200)]
    public string? Key { get; set; }

    [Column("fld_Category")]
    [StringLength(150)]
    public string? Category { get; set; }

    [Column("fld_Tags")]
    [StringLength(1000)]
    public string? Tags { get; set; }

    [Required]
    [Column("fld_FileHash")]
    [StringLength(128)]
    public string FileHash { get; set; }

    [Column("fld_FileSize")]
    public long FileSize { get; set; }

    [Required]
    [Column("fld_ProcessingStatus")]
    [StringLength(50)]
    public string ProcessingStatus { get; set; } = RagProcessingStatus.Pending;

    [Column("fld_ProcessingError")]
    [StringLength(2000)]
    public string? ProcessingError { get; set; }

    [Column("fld_ChunkCount")]
    public int ChunkCount { get; set; }

    [Column("fld_CreateDateTime")]
    public DateTime CreateDateTime { get; set; }

    [Column("fld_CreatorUserId")]
    [StringLength(100)]
    public string? CreatorUserId { get; set; }

    [Column("fld_LastUpdateDateTime")]
    public DateTime? LastUpdateDateTime { get; set; }

    [Column("fld_LastUpdateUserId")]
    [StringLength(100)]
    public string? LastUpdateUserId { get; set; }

    public ICollection<RagDocumentChunk> Chunks { get; set; } = new List<RagDocumentChunk>();
}

public static class RagProcessingStatus
{
    public const string Pending = "Pending";
    public const string Processing = "Processing";
    public const string Completed = "Completed";
    public const string Failed = "Failed";
}
