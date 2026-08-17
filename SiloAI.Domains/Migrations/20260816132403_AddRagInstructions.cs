using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiloAI.Domains.Migrations
{
    /// <inheritdoc />
    public partial class AddRagInstructions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_RagInstructions",
                columns: table => new
                {
                    fld_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fld_DocType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "GeneralChat"),
                    fld_Key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    fld_Category = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    fld_Tags = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    fld_Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fld_IsActive = table.Column<bool>(type: "bit", nullable: false),
                    fld_CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fld_CreatorUserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fld_LastUpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fld_LastUpdateUserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RagInstructions", x => x.fld_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RagInstructions_fld_Category",
                table: "tbl_RagInstructions",
                column: "fld_Category");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RagInstructions_fld_DocType",
                table: "tbl_RagInstructions",
                column: "fld_DocType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_RagInstructions");
        }
    }
}
