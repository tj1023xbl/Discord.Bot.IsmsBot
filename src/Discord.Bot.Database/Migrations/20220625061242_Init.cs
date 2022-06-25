using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Discord.Bot.Database.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Sayings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsmSaying = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsmRecorder = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sayings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sayings_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sayings_IsmSaying",
                table: "Sayings",
                column: "IsmSaying",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sayings_Username",
                table: "Sayings",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GuildId_Username",
                table: "Users",
                columns: new[] { "GuildId", "Username" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sayings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
