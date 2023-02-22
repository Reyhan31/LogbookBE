using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogBookAPI.Migrations
{
    public partial class secondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gaji",
                table: "Logbook",
                newName: "GajiWFO");

            migrationBuilder.AddColumn<long>(
                name: "GajiTotal",
                table: "Logbook",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GajiWFH",
                table: "Logbook",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GajiTotal",
                table: "Logbook");

            migrationBuilder.DropColumn(
                name: "GajiWFH",
                table: "Logbook");

            migrationBuilder.RenameColumn(
                name: "GajiWFO",
                table: "Logbook",
                newName: "Gaji");
        }
    }
}
