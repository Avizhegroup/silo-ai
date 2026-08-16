namespace SiloAI.Domains;

[Table("tbl_AiConversations")]
public class AiConversation
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("fld_Id")]
    public int Id { get; set; }

    [Column("fld_UserAsk")]
    public string UserAsk { get; set; }

    [Column("fld_BotAnswer")]
    public string BotAnswer { get; set; }

    [Column("fld_InstructionKey")]
    [StringLength(500)]
    public string? InstructionKey { get; set; }

    [Column("fld_CreditUsage")]
    public long? CreditUsage { get; set; }

    [Column("fld_LocalConversationId")]
    public int LocalConversationId { get; set; }

    [Column("fld_CustomerId")]
    public int CustomerId { get; set; }

    [Column("fld_CreatedAt")]
    public DateTime CreatedAt { get; set; }
}
