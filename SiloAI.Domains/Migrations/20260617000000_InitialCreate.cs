using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiloAI.Domains.Migrations
{
    [DbContext(typeof(AiApiContext))]
    [Migration("20260617000000_InitialCreate")]
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_AiAdminUsers",
                columns: table => new
                {
                    fld_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fld_Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fld_PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    fld_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    fld_IsActive = table.Column<bool>(type: "bit", nullable: false),
                    fld_CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AiAdminUsers", x => x.fld_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_AiCustomers",
                columns: table => new
                {
                    fld_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fld_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    fld_RemainingCredit = table.Column<long>(type: "bigint", nullable: false),
                    fld_CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AiCustomers", x => x.fld_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_AiConversations",
                columns: table => new
                {
                    fld_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fld_UserAsk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fld_BotAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fld_InstructionKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    fld_CreditUsage = table.Column<long>(type: "bigint", nullable: true),
                    fld_LocalConversationId = table.Column<int>(type: "int", nullable: false),
                    fld_CustomerId = table.Column<int>(type: "int", nullable: false),
                    fld_CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AiConversations", x => x.fld_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_AiApiKeys",
                columns: table => new
                {
                    fld_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fld_KeyValue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    fld_Label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    fld_ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fld_IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    fld_CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fld_CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AiApiKeys", x => x.fld_Id);
                    table.ForeignKey(
                        name: "FK_tbl_AiApiKeys_tbl_AiCustomers_fld_CustomerId",
                        column: x => x.fld_CustomerId,
                        principalTable: "tbl_AiCustomers",
                        principalColumn: "fld_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AiApiKeys_fld_CustomerId",
                table: "tbl_AiApiKeys",
                column: "fld_CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "tbl_AiApiKeys");
            migrationBuilder.DropTable(name: "tbl_AiConversations");
            migrationBuilder.DropTable(name: "tbl_AiCustomers");
            migrationBuilder.DropTable(name: "tbl_AiAdminUsers");
        }
    }
}
