using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaobaoAuthorization.Migrations.TaobaoAuthorizedDb
{
    public partial class TaobaoAuthed_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorizedInfos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    AppKey = table.Column<long>(nullable: false),
                    AuthState = table.Column<string>(type: "varchar(100)", nullable: false),
                    AccessToken = table.Column<string>(type: "varchar(100)", nullable: false),
                    ExpiresTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizedInfos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedInfos_AppKey_AuthState",
                table: "AuthorizedInfos",
                columns: new[] { "AppKey", "AuthState" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizedInfos");
        }
    }
}
