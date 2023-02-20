using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Discord.Bot.Database.Migrations
{
    public partial class addguild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sayings_Users_UserIsmKey",
                table: "Sayings");

            migrationBuilder.AlterColumn<string>(
                name: "IsmRecorder",
                table: "Sayings",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "GuildId",
                table: "Sayings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<string>(
                name: "IsmKey",
                table: "Sayings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                "PRAGMA foreign_keys = 0; " +
                "CREATE TABLE \"manual_temp_Sayings\" (" +
                    "\"Id\" TEXT NOT NULL CONSTRAINT \"PK_Sayings\" PRIMARY KEY, " +
                    "\"DateCreated\" TEXT NOT NULL, " +
                    "\"GuildId\" INTEGER NOT NULL, " +
                    "\"IsmKey\" TEXT NOT NULL, " +
                    "\"IsmRecorder\" TEXT NOT NULL, " +
                    "\"IsmSaying\" TEXT NOT NULL " +
                    "); " +
                "INSERT INTO \"manual_temp_Sayings\" (\"Id\", \"DateCreated\", \"GuildId\", \"IsmKey\", \"IsmRecorder\", \"IsmSaying\") " +
                    "SELECT \"Id\", \"DateCreated\", Users.GuildId, Users.IsmKey, IFNULL(\"IsmRecorder\", ''), \"IsmSaying\" " +
                    "FROM Sayings " +
                    "JOIN Users ON Users.IsmKey = Sayings.UserIsmKey; " +
                "DROP TABLE \"Sayings\"; " +
                "ALTER TABLE \"manual_temp_Sayings\" RENAME TO \"Sayings\"; " +
                "PRAGMA foreign_keys = 1;"
                );

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropColumn(
                name: "UserIsmKey",
                table: "Sayings");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "Sayings");

            migrationBuilder.DropColumn(
                name: "IsmKey",
                table: "Sayings");

            migrationBuilder.AlterColumn<string>(
                name: "IsmRecorder",
                table: "Sayings",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "UserIsmKey",
                table: "Sayings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IsmKey = table.Column<string>(type: "TEXT", nullable: false),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IsmKey);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sayings_UserIsmKey",
                table: "Sayings",
                column: "UserIsmKey");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GuildId_IsmKey",
                table: "Users",
                columns: new[] { "GuildId", "IsmKey" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sayings_Users_UserIsmKey",
                table: "Sayings",
                column: "UserIsmKey",
                principalTable: "Users",
                principalColumn: "IsmKey");
        }
    }
}
