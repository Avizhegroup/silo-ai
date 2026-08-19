namespace SiloAI.Domains;

/// <summary>
/// Server-side persisted Microsoft Agent Framework session state for a conversation.
/// The client only ever knows the <see cref="Id"/> (conversationId); the serialized
/// agent session state never leaves the API.
/// </summary>
[Table("tbl_AiChatSessions")]
public class AiChatSession
{
    [Key]
    [Column("fld_Id")]
    public Guid Id { get; set; }

    [Required]
    [Column("fld_OwnerKey")]
    [StringLength(200)]
    public string OwnerKey { get; set; }

    [Required]
    [Column("fld_ChatType")]
    [StringLength(50)]
    public string ChatType { get; set; }

    [Column("fld_SessionState")]
    public string SessionState { get; set; }

    [Column("fld_CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [Column("fld_UpdatedAt")]
    public DateTime UpdatedAt { get; set; }
}
