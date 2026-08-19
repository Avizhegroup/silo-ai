namespace SiloAI.Domains;

public class AiApiContext(DbContextOptions<AiApiContext> options) : DbContext(options)
{
    public DbSet<AiApiKey> AiApiKeys { get; set; }
    public DbSet<AiAdminUser> AiAdminUsers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<AiConversation> AiConversations { get; set; }
    public DbSet<RagDocument> RagDocuments { get; set; }
    public DbSet<RagDocumentChunk> RagDocumentChunks { get; set; }
    public DbSet<RagInstruction> RagInstructions { get; set; }
    public DbSet<AiChatSession> AiChatSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RagDocument>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.DocType).HasDefaultValue("GeneralChat");
            b.HasIndex(x => x.FileHash).HasDatabaseName("IX_tbl_RagDocuments_fld_FileHash");
            b.HasIndex(x => x.Category).HasDatabaseName("IX_tbl_RagDocuments_fld_Category");
            b.HasIndex(x => x.DocType).HasDatabaseName("IX_tbl_RagDocuments_fld_DocType");
        });

        modelBuilder.Entity<RagDocumentChunk>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasOne(x => x.Document)
                .WithMany(d => d.Chunks)
                .HasForeignKey(x => x.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(x => x.DocumentId).HasDatabaseName("IX_tbl_RagDocumentChunks_fld_DocumentId");
            b.HasIndex(x => x.ChunkIndex).HasDatabaseName("IX_tbl_RagDocumentChunks_fld_ChunkIndex");
        });

        modelBuilder.Entity<RagInstruction>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.DocType).HasDefaultValue("GeneralChat");
            b.HasIndex(x => x.DocType).HasDatabaseName("IX_tbl_RagInstructions_fld_DocType");
            b.HasIndex(x => x.Category).HasDatabaseName("IX_tbl_RagInstructions_fld_Category");
        });

        modelBuilder.Entity<AiChatSession>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.OwnerKey).HasDatabaseName("IX_tbl_AiChatSessions_fld_OwnerKey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(x => x.RemainingCredit)
                .HasColumnName("fld_RemainingCredit")
                .HasColumnType("decimal(18,8)")
                .IsRequired();
        });
    }
}
