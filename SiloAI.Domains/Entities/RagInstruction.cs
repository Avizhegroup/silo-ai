namespace SiloAI.Domains;

[Table("tbl_RagInstructions")]
public class RagInstruction
{
    [Column("fld_Id")]
    public Guid Id { get; set; }

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
    [Column("fld_Content")]
    public string Content { get; set; }

    [Column("fld_IsActive")]
    public bool IsActive { get; set; } = true;

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
}
