using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop_CookInStyle.Migrations
{
    public partial class AanpassingKlant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WachtWoordReset",
                table: "Klanten",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WachtWoordReset",
                table: "Klanten");
        }
    }
}
