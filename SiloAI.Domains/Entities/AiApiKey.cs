namespace SiloAI.Domains;

[Table("tbl_AiApiKeys")]
public class AiApiKey
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("fld_Id")]
    public int Id { get; set; }

    [Required]
    [Column("fld_KeyValue")]
    [StringLength(256)]
    public string KeyValue { get; set; }

    [Required]
    [Column("fld_Label")]
    [StringLength(200)]
    public string Label { get; set; }

    [Required]
    [Column("fld_ExpiresAt")]
    public DateTime ExpiresAt { get; set; }

    [Required]
    [Column("fld_IsRevoked")]
    public bool IsRevoked { get; set; }

    [Required]
    [Column("fld_CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [Column("fld_CustomerId")]
    public int? CustomerId { get; set; }

    public Customer? Customer { get; set; }
}
