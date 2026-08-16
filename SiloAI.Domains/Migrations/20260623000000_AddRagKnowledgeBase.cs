using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiloAI.Domains.Migrations
{
    /// <summary>
    /// Adds the RAG knowledge base tables. The chunk embedding column uses the SQL Server 2025
    /// native <c>VECTOR</c> data type and is added via raw SQL because EF Core 9 does not yet
    /// model the <c>VECTOR</c> type directly.
    /// </summary>
    [DbContext(typeof(AiApiContext))]
    [Migration("20260623000000_AddRagKnowledgeBase")]
    public partial class AddRagKnowledgeBase : Migration
    {
        // Default OpenAI text-embedding-3-small dimensionality. Adjust by configuration/migration
        // when switching to a model with a different vector size (e.g. 3072 for -3-large).
        private const int EmbeddingDimensions = 1536;

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_RagDocuments",
                columns: table => new
                {
                    fld_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fld_FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    fld_OriginalFileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    fld_ContentType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    fld_Category = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    fld_Tags = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    fld_FileHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    fld_FileSize = table.Column<long>(type: "bigint", nullable: false),
                    fld_ProcessingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fld_ProcessingError = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    fld_ChunkCount = table.Column<int>(type: "int", nullable: false),
                    fld_CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fld_CreatorUserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fld_LastUpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fld_LastUpdateUserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RagDocuments", x => x.fld_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_RagDocumentChunks",
                columns: table => new
                {
                    fld_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fld_DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fld_ChunkIndex = table.Column<int>(type: "int", nullable: false),
                    fld_Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fld_TokenCount = table.Column<int>(type: "int", nullable: false),
                    fld_CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RagDocumentChunks", x => x.fld_Id);
                    table.ForeignKey(
                        name: "FK_tbl_RagDocumentChunks_tbl_RagDocuments_fld_DocumentId",
                        column: x => x.fld_DocumentId,
                        principalTable: "tbl_RagDocuments",
                        principalColumn: "fld_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RagDocuments_fld_FileHash",
                table: "tbl_RagDocuments",
                column: "fld_FileHash");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RagDocuments_fld_Category",
                table: "tbl_RagDocuments",
                column: "fld_Category");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RagDocumentChunks_fld_DocumentId",
                table: "tbl_RagDocumentChunks",
                column: "fld_DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RagDocumentChunks_fld_ChunkIndex",
                table: "tbl_RagDocumentChunks",
                column: "fld_ChunkIndex");

            // SQL Server 2025 native VECTOR column. Added via raw SQL because the EF Core
            // SqlServer provider does not yet expose a typed column builder for VECTOR.
            migrationBuilder.Sql(
                $"ALTER TABLE [tbl_RagDocumentChunks] ADD [fld_Embedding] VECTOR({EmbeddingDimensions}) NULL;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "tbl_RagDocumentChunks");
            migrationBuilder.DropTable(name: "tbl_RagDocuments");
        }
    }
}
