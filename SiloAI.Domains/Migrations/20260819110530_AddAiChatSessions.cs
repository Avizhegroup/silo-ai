using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiloAI.Domains.Migrations
{
    /// <inheritdoc />
    public partial class AddAiChatSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_AiChatSessions",
                columns: table => new
                {
                    fld_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fld_OwnerKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    fld_ChatType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fld_SessionState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fld_CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fld_UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AiChatSessions", x => x.fld_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AiChatSessions_fld_OwnerKey",
                table: "tbl_AiChatSessions",
                column: "fld_OwnerKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_AiChatSessions");
        }
    }
}
