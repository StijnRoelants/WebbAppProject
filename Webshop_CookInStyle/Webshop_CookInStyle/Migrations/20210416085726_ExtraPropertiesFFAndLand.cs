using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop_CookInStyle.Migrations
{
    public partial class ExtraPropertiesFFAndLand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Zoeknaam",
                table: "Landen",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Factuurnummering",
                table: "Facturatiefirmas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zoeknaam",
                table: "Landen");

            migrationBuilder.DropColumn(
                name: "Factuurnummering",
                table: "Facturatiefirmas");
        }
    }
}
