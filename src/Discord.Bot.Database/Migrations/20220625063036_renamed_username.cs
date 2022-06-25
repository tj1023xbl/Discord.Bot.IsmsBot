using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Discord.Bot.Database.Migrations
{
    public partial class renamed_username : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sayings_Users_Username",
                table: "Sayings");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "IsmKey");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GuildId_Username",
                table: "Users",
                newName: "IX_Users_GuildId_IsmKey");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Sayings",
                newName: "UserIsmKey");

            migrationBuilder.RenameIndex(
                name: "IX_Sayings_Username",
                table: "Sayings",
                newName: "IX_Sayings_UserIsmKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Sayings_Users_UserIsmKey",
                table: "Sayings",
                column: "UserIsmKey",
                principalTable: "Users",
                principalColumn: "IsmKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sayings_Users_UserIsmKey",
                table: "Sayings");

            migrationBuilder.RenameColumn(
                name: "IsmKey",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GuildId_IsmKey",
                table: "Users",
                newName: "IX_Users_GuildId_Username");

            migrationBuilder.RenameColumn(
                name: "UserIsmKey",
                table: "Sayings",
                newName: "Username");

            migrationBuilder.RenameIndex(
                name: "IX_Sayings_UserIsmKey",
                table: "Sayings",
                newName: "IX_Sayings_Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Sayings_Users_Username",
                table: "Sayings",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username");
        }
    }
}
