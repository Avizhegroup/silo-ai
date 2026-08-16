namespace SiloAI.Domains;

[Table("tbl_RagDocumentChunks")]
public class RagDocumentChunk
{
    [Column("fld_Id")]
    public Guid Id { get; set; }

    [Column("fld_DocumentId")]
    public Guid DocumentId { get; set; }

    public RagDocument? Document { get; set; }

    [Column("fld_ChunkIndex")]
    public int ChunkIndex { get; set; }

    [Required]
    [Column("fld_Content")]
    public string Content { get; set; }

    [Column("fld_TokenCount")]
    public int TokenCount { get; set; }

    [Column("fld_CreateDateTime")]
    public DateTime CreateDateTime { get; set; }

    /// <summary>
    /// Stored in SQL Server 2025 as a native VECTOR(N) column. Excluded from EF model and
    /// written/read via raw SQL by the indexing/search services.
    /// </summary>
    [NotMapped]
    public float[]? Embedding { get; set; }
}
