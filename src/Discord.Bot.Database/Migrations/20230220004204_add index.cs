using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Discord.Bot.Database.Migrations
{
    public partial class addindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sayings_IsmKey_GuildId",
                table: "Sayings",
                columns: new[] { "IsmKey", "GuildId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sayings_IsmKey_GuildId",
                table: "Sayings");
        }
    }
}
