using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop_CookInStyle.Migrations
{
    public partial class iHopeThisFixesIt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Klanten_KlantId",
                table: "Bestellingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_LeverAdressen_LeverAdresID1",
                table: "Bestellingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturen_Bestellingen_BestellingID1",
                table: "Facturen");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturen_Facturatiefirmas_FactuurfirmaID1",
                table: "Facturen");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturen_Klanten_KlantId",
                table: "Facturen");

            migrationBuilder.DropForeignKey(
                name: "FK_LeverAdressen_Klanten_KlantId",
                table: "LeverAdressen");

            migrationBuilder.DropForeignKey(
                name: "FK_LeverAdressen_Landen_LandID1",
                table: "LeverAdressen");

            migrationBuilder.DropForeignKey(
                name: "FK_LeverAdressen_Postcodes_PostcodeID1",
                table: "LeverAdressen");

            migrationBuilder.DropIndex(
                name: "IX_LeverAdressen_KlantFK",
                table: "LeverAdressen");

            migrationBuilder.DropIndex(
                name: "IX_LeverAdressen_KlantId",
                table: "LeverAdressen");

            migrationBuilder.DropIndex(
                name: "IX_LeverAdressen_LandID",
                table: "LeverAdressen");

            migrationBuilder.DropIndex(
                name: "IX_LeverAdressen_LandID1",
                table: "LeverAdressen");

            migrationBuilder.DropIndex(
                name: "IX_LeverAdressen_PostcodeID",
                table: "LeverAdressen");

            migrationBuilder.DropIndex(
                name: "IX_LeverAdressen_PostcodeID1",
                table: "LeverAdressen");

            migrationBuilder.DropIndex(
                name: "IX_Facturen_BestellingID",
                table: "Facturen");

            migrationBuilder.DropIndex(
                name: "IX_Facturen_BestellingID1",
                table: "Facturen");

            migrationBuilder.DropIndex(
                name: "IX_Facturen_FactuurfirmaID",
                table: "Facturen");

            migrationBuilder.DropIndex(
                name: "IX_Facturen_FactuurfirmaID1",
                table: "Facturen");

            migrationBuilder.DropIndex(
                name: "IX_Facturen_KlantFK",
                table: "Facturen");

            migrationBuilder.DropIndex(
                name: "IX_Facturen_KlantId",
                table: "Facturen");

            migrationBuilder.DropIndex(
                name: "IX_Bestellingen_KlantFK",
                table: "Bestellingen");

            migrationBuilder.DropIndex(
                name: "IX_Bestellingen_KlantId",
                table: "Bestellingen");

            migrationBuilder.DropIndex(
                name: "IX_Bestellingen_LeverAdresID",
                table: "Bestellingen");

            migrationBuilder.DropIndex(
                name: "IX_Bestellingen_LeverAdresID1",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "KlantId",
                table: "LeverAdressen");

            migrationBuilder.DropColumn(
                name: "LandID1",
                table: "LeverAdressen");

            migrationBuilder.DropColumn(
                name: "PostcodeID1",
                table: "LeverAdressen");

            migrationBuilder.DropColumn(
                name: "BestellingID1",
                table: "Facturen");

            migrationBuilder.DropColumn(
                name: "FactuurfirmaID1",
                table: "Facturen");

            migrationBuilder.DropColumn(
                name: "KlantId",
                table: "Facturen");

            migrationBuilder.DropColumn(
                name: "KlantId",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "LeverAdresID1",
                table: "Bestellingen");

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_KlantFK",
                table: "LeverAdressen",
                column: "KlantFK");

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_LandID",
                table: "LeverAdressen",
                column: "LandID");

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_PostcodeID",
                table: "LeverAdressen",
                column: "PostcodeID");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_BestellingID",
                table: "Facturen",
                column: "BestellingID");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_FactuurfirmaID",
                table: "Facturen",
                column: "FactuurfirmaID");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_KlantFK",
                table: "Facturen",
                column: "KlantFK");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_KlantFK",
                table: "Bestellingen",
                column: "KlantFK");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_LeverAdresID",
                table: "Bestellingen",
                column: "LeverAdresID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeverAdressen_KlantFK",
                table: "LeverAdressen");

            migrationBuilder.DropIndex(
                name: "IX_LeverAdressen_LandID",
                table: "LeverAdressen");

            migrationBuilder.DropIndex(
                name: "IX_LeverAdressen_PostcodeID",
                table: "LeverAdressen");

            migrationBuilder.DropIndex(
                name: "IX_Facturen_BestellingID",
                table: "Facturen");

            migrationBuilder.DropIndex(
                name: "IX_Facturen_FactuurfirmaID",
                table: "Facturen");

            migrationBuilder.DropIndex(
                name: "IX_Facturen_KlantFK",
                table: "Facturen");

            migrationBuilder.DropIndex(
                name: "IX_Bestellingen_KlantFK",
                table: "Bestellingen");

            migrationBuilder.DropIndex(
                name: "IX_Bestellingen_LeverAdresID",
                table: "Bestellingen");

            migrationBuilder.AddColumn<string>(
                name: "KlantId",
                table: "LeverAdressen",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LandID1",
                table: "LeverAdressen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostcodeID1",
                table: "LeverAdressen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BestellingID1",
                table: "Facturen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FactuurfirmaID1",
                table: "Facturen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KlantId",
                table: "Facturen",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KlantId",
                table: "Bestellingen",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeverAdresID1",
                table: "Bestellingen",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_KlantFK",
                table: "LeverAdressen",
                column: "KlantFK",
                unique: true,
                filter: "[KlantFK] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_KlantId",
                table: "LeverAdressen",
                column: "KlantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_LandID",
                table: "LeverAdressen",
                column: "LandID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_LandID1",
                table: "LeverAdressen",
                column: "LandID1");

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_PostcodeID",
                table: "LeverAdressen",
                column: "PostcodeID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_PostcodeID1",
                table: "LeverAdressen",
                column: "PostcodeID1");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_BestellingID",
                table: "Facturen",
                column: "BestellingID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_BestellingID1",
                table: "Facturen",
                column: "BestellingID1");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_FactuurfirmaID",
                table: "Facturen",
                column: "FactuurfirmaID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_FactuurfirmaID1",
                table: "Facturen",
                column: "FactuurfirmaID1");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_KlantFK",
                table: "Facturen",
                column: "KlantFK",
                unique: true,
                filter: "[KlantFK] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_KlantId",
                table: "Facturen",
                column: "KlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_KlantFK",
                table: "Bestellingen",
                column: "KlantFK",
                unique: true,
                filter: "[KlantFK] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_KlantId",
                table: "Bestellingen",
                column: "KlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_LeverAdresID",
                table: "Bestellingen",
                column: "LeverAdresID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_LeverAdresID1",
                table: "Bestellingen",
                column: "LeverAdresID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Klanten_KlantId",
                table: "Bestellingen",
                column: "KlantId",
                principalTable: "Klanten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_LeverAdressen_LeverAdresID1",
                table: "Bestellingen",
                column: "LeverAdresID1",
                principalTable: "LeverAdressen",
                principalColumn: "LeverAdresID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturen_Bestellingen_BestellingID1",
                table: "Facturen",
                column: "BestellingID1",
                principalTable: "Bestellingen",
                principalColumn: "BestellingID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturen_Facturatiefirmas_FactuurfirmaID1",
                table: "Facturen",
                column: "FactuurfirmaID1",
                principalTable: "Facturatiefirmas",
                principalColumn: "FactuurfirmaID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturen_Klanten_KlantId",
                table: "Facturen",
                column: "KlantId",
                principalTable: "Klanten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeverAdressen_Klanten_KlantId",
                table: "LeverAdressen",
                column: "KlantId",
                principalTable: "Klanten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeverAdressen_Landen_LandID1",
                table: "LeverAdressen",
                column: "LandID1",
                principalTable: "Landen",
                principalColumn: "LandID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeverAdressen_Postcodes_PostcodeID1",
                table: "LeverAdressen",
                column: "PostcodeID1",
                principalTable: "Postcodes",
                principalColumn: "PostcodeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
