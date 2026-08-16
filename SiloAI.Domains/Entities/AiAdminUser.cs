namespace SiloAI.Domains;

[Table("tbl_AiAdminUsers")]
public class AiAdminUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("fld_Id")]
    public int Id { get; set; }

    [Required]
    [Column("fld_Username")]
    [StringLength(100)]
    public string Username { get; set; }

    [Required]
    [Column("fld_PasswordHash")]
    [StringLength(256)]
    public string PasswordHash { get; set; }

    [Required]
    [Column("fld_Name")]
    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    [Column("fld_IsActive")]
    public bool IsActive { get; set; }

    [Required]
    [Column("fld_CreatedAt")]
    public DateTime CreatedAt { get; set; }
}
