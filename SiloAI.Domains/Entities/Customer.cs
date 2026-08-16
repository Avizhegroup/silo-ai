namespace SiloAI.Domains;

[Table("tbl_AiCustomers")]
public class Customer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("fld_Id")]
    public int Id { get; set; }

    [Required]
    [Column("fld_Name")]
    [StringLength(200)]
    public string Name { get; set; }

    [Column("fld_RemainingCredit")]
    public decimal RemainingCredit { get; set; }

    [Column("fld_CreatedAt")]
    public DateTime CreatedAt { get; set; }

    public ICollection<AiApiKey> AiApiKeys { get; set; } = new List<AiApiKey>();
}
