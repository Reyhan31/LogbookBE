using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogBookAPI.Migrations
{
    public partial class alterMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "mobileNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_MentorId",
                table: "Users",
                column: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_MentorId",
                table: "Users",
                column: "MentorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_MentorId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_MentorId",
                table: "Users");

            migrationBuilder.AlterColumn<long>(
                name: "mobileNumber",
                table: "Users",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
