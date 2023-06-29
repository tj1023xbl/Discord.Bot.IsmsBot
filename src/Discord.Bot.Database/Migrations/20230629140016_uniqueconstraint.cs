using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Discord.Bot.Database.Migrations
{
    public partial class uniqueconstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sayings_IsmSaying",
                table: "Sayings");

            migrationBuilder.CreateIndex(
                name: "IX_Sayings_IsmSaying_GuildId",
                table: "Sayings",
                columns: new[] { "IsmSaying", "GuildId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sayings_IsmSaying_GuildId",
                table: "Sayings");

            migrationBuilder.CreateIndex(
                name: "IX_Sayings_IsmSaying",
                table: "Sayings",
                column: "IsmSaying",
                unique: true);
        }
    }
}
