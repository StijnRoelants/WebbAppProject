using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop_CookInStyle.Migrations
{
    public partial class AanpassingenFfEnBestelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bestelbonnummering",
                table: "Facturatiefirmas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bestelbonnummer",
                table: "Bestellingen",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bestelbonnummering",
                table: "Facturatiefirmas");

            migrationBuilder.DropColumn(
                name: "Bestelbonnummer",
                table: "Bestellingen");
        }
    }
}
