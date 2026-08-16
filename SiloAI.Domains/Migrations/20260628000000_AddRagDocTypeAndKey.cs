using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiloAI.Domains.Migrations
{
    [DbContext(typeof(AiApiContext))]
    [Migration("20260628000000_AddRagDocTypeAndKey")]
    public partial class AddRagDocTypeAndKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fld_DocType",
                table: "tbl_RagDocuments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "GeneralChat");

            migrationBuilder.AddColumn<string>(
                name: "fld_Key",
                table: "tbl_RagDocuments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RagDocuments_fld_DocType",
                table: "tbl_RagDocuments",
                column: "fld_DocType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbl_RagDocuments_fld_DocType",
                table: "tbl_RagDocuments");

            migrationBuilder.DropColumn(
                name: "fld_DocType",
                table: "tbl_RagDocuments");

            migrationBuilder.DropColumn(
                name: "fld_Key",
                table: "tbl_RagDocuments");
        }
    }
}
