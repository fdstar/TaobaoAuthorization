using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaobaoAuthorization.Migrations
{
    public partial class TaobaoAuth_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthOrders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    PartnerId = table.Column<long>(nullable: false),
                    AppKey = table.Column<long>(nullable: false),
                    AuthState = table.Column<string>(type: "varchar(100)", nullable: false),
                    AuthView = table.Column<string>(type: "varchar(5)", nullable: false),
                    RedirectUri = table.Column<string>(type: "varchar(150)", nullable: false),
                    TaobaoCode = table.Column<string>(type: "varchar(100)", nullable: false),
                    Error = table.Column<string>(maxLength: 20, nullable: false),
                    ErrorDescription = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    PartnerKey = table.Column<string>(type: "varchar(50)", nullable: false),
                    SecretKey = table.Column<string>(type: "varchar(100)", nullable: false),
                    PartnerName = table.Column<string>(maxLength: 100, nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthOrders_PartnerId_AppKey_AuthState",
                table: "AuthOrders",
                columns: new[] { "PartnerId", "AppKey", "AuthState" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Partners_PartnerKey",
                table: "Partners",
                column: "PartnerKey",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthOrders");

            migrationBuilder.DropTable(
                name: "Partners");
        }
    }
}
