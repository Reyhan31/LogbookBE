using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogBookAPI.Migrations
{
    public partial class thirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "mobileNumber",
                table: "Users",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mobileNumber",
                table: "Users");
        }
    }
}
