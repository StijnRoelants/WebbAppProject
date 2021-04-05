using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop_CookInStyle.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allergenen",
                columns: table => new
                {
                    AllergeenID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Omschrijving = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergenen", x => x.AllergeenID);
                });

            migrationBuilder.CreateTable(
                name: "BtwTypes",
                columns: table => new
                {
                    BtwID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Percentage = table.Column<int>(nullable: false),
                    Omschrijving = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BtwTypes", x => x.BtwID);
                });

            migrationBuilder.CreateTable(
                name: "Eenheden",
                columns: table => new
                {
                    EenheidID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Omschrijving = table.Column<string>(nullable: true),
                    Afkorting = table.Column<string>(maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eenheden", x => x.EenheidID);
                });

            migrationBuilder.CreateTable(
                name: "Landen",
                columns: table => new
                {
                    LandID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(nullable: true),
                    Afkorting = table.Column<string>(maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landen", x => x.LandID);
                });

            migrationBuilder.CreateTable(
                name: "Postcodes",
                columns: table => new
                {
                    PostcodeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gemeente = table.Column<string>(nullable: true),
                    Nummer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcodes", x => x.PostcodeID);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    ProductTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Omschrijving = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.ProductTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Facturatiefirmas",
                columns: table => new
                {
                    FactuurfirmaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(nullable: true),
                    Straat = table.Column<string>(nullable: true),
                    Iban = table.Column<string>(nullable: true),
                    Bic = table.Column<string>(nullable: true),
                    Banknaam = table.Column<string>(nullable: true),
                    BtwNummer = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Telefoon = table.Column<string>(nullable: true),
                    PostcodeID = table.Column<int>(nullable: false),
                    LandID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturatiefirmas", x => x.FactuurfirmaID);
                    table.ForeignKey(
                        name: "FK_Facturatiefirmas_Landen_LandID",
                        column: x => x.LandID,
                        principalTable: "Landen",
                        principalColumn: "LandID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facturatiefirmas_Postcodes_PostcodeID",
                        column: x => x.PostcodeID,
                        principalTable: "Postcodes",
                        principalColumn: "PostcodeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Klanten",
                columns: table => new
                {
                    KlantID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(nullable: true),
                    Voornaam = table.Column<string>(nullable: true),
                    Achternaam = table.Column<string>(nullable: true),
                    StraatEnNummer = table.Column<string>(nullable: true),
                    IsBedrijf = table.Column<bool>(nullable: false),
                    BtwNummer = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Mobiel = table.Column<string>(nullable: true),
                    Telefoon = table.Column<string>(nullable: true),
                    PostcodeID = table.Column<int>(nullable: false),
                    LandID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klanten", x => x.KlantID);
                    table.ForeignKey(
                        name: "FK_Klanten_Landen_LandID",
                        column: x => x.LandID,
                        principalTable: "Landen",
                        principalColumn: "LandID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Klanten_Postcodes_PostcodeID",
                        column: x => x.PostcodeID,
                        principalTable: "Postcodes",
                        principalColumn: "PostcodeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producten",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(nullable: true),
                    Omschrijving = table.Column<string>(nullable: true),
                    Eenheidsprijs = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    BeschikbaarInWebshop = table.Column<bool>(nullable: false),
                    EenheidID = table.Column<int>(nullable: false),
                    ProductTypeID = table.Column<int>(nullable: false),
                    BtwID = table.Column<int>(nullable: false),
                    BtwtypeBtwID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producten", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Producten_BtwTypes_BtwtypeBtwID",
                        column: x => x.BtwtypeBtwID,
                        principalTable: "BtwTypes",
                        principalColumn: "BtwID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Producten_Eenheden_EenheidID",
                        column: x => x.EenheidID,
                        principalTable: "Eenheden",
                        principalColumn: "EenheidID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producten_ProductTypes_ProductTypeID",
                        column: x => x.ProductTypeID,
                        principalTable: "ProductTypes",
                        principalColumn: "ProductTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeverAdressen",
                columns: table => new
                {
                    LeverAdresID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Straat = table.Column<string>(nullable: true),
                    Omschrijving = table.Column<string>(nullable: true),
                    PostcodeID = table.Column<int>(nullable: false),
                    LandID = table.Column<int>(nullable: false),
                    KlantID = table.Column<int>(nullable: false),
                    KlantID1 = table.Column<int>(nullable: true),
                    LandID1 = table.Column<int>(nullable: true),
                    PostcodeID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeverAdressen", x => x.LeverAdresID);
                    table.ForeignKey(
                        name: "FK_LeverAdressen_Klanten_KlantID",
                        column: x => x.KlantID,
                        principalTable: "Klanten",
                        principalColumn: "KlantID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeverAdressen_Klanten_KlantID1",
                        column: x => x.KlantID1,
                        principalTable: "Klanten",
                        principalColumn: "KlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeverAdressen_Landen_LandID",
                        column: x => x.LandID,
                        principalTable: "Landen",
                        principalColumn: "LandID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeverAdressen_Landen_LandID1",
                        column: x => x.LandID1,
                        principalTable: "Landen",
                        principalColumn: "LandID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeverAdressen_Postcodes_PostcodeID",
                        column: x => x.PostcodeID,
                        principalTable: "Postcodes",
                        principalColumn: "PostcodeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeverAdressen_Postcodes_PostcodeID1",
                        column: x => x.PostcodeID1,
                        principalTable: "Postcodes",
                        principalColumn: "PostcodeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AllergeenProducten",
                columns: table => new
                {
                    AllergeenProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllergeenID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllergeenProducten", x => x.AllergeenProductID);
                    table.ForeignKey(
                        name: "FK_AllergeenProducten_Allergenen_AllergeenID",
                        column: x => x.AllergeenID,
                        principalTable: "Allergenen",
                        principalColumn: "AllergeenID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllergeenProducten_Producten_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Producten",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bestellingen",
                columns: table => new
                {
                    BestellingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BestelDatum = table.Column<DateTime>(nullable: false),
                    Leverdatum = table.Column<DateTime>(nullable: false),
                    Totaalprijs = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Opmerking = table.Column<string>(nullable: true),
                    KlantID = table.Column<int>(nullable: false),
                    LeverAdresID = table.Column<int>(nullable: false),
                    KlantID1 = table.Column<int>(nullable: true),
                    LeverAdresID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bestellingen", x => x.BestellingID);
                    table.ForeignKey(
                        name: "FK_Bestellingen_Klanten_KlantID",
                        column: x => x.KlantID,
                        principalTable: "Klanten",
                        principalColumn: "KlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bestellingen_Klanten_KlantID1",
                        column: x => x.KlantID1,
                        principalTable: "Klanten",
                        principalColumn: "KlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bestellingen_LeverAdressen_LeverAdresID",
                        column: x => x.LeverAdresID,
                        principalTable: "LeverAdressen",
                        principalColumn: "LeverAdresID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bestellingen_LeverAdressen_LeverAdresID1",
                        column: x => x.LeverAdresID1,
                        principalTable: "LeverAdressen",
                        principalColumn: "LeverAdresID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bestellijnen",
                columns: table => new
                {
                    BestellijnID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aantal = table.Column<int>(nullable: false),
                    Lijnprijs = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Eenheidsprijs = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    BtwBedrag = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    BestellingID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bestellijnen", x => x.BestellijnID);
                    table.ForeignKey(
                        name: "FK_Bestellijnen_Bestellingen_BestellingID",
                        column: x => x.BestellingID,
                        principalTable: "Bestellingen",
                        principalColumn: "BestellingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bestellijnen_Producten_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Producten",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facturen",
                columns: table => new
                {
                    FactuurID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Factuurnummer = table.Column<string>(nullable: true),
                    FactuurDatum = table.Column<DateTime>(nullable: false),
                    TotaalPrijsEx = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TotaalPrijsIncl = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    BtwnummerKlant = table.Column<string>(nullable: true),
                    BtwnummerFacFirma = table.Column<string>(nullable: true),
                    IsBetaald = table.Column<bool>(nullable: false),
                    NaamKlant = table.Column<string>(nullable: true),
                    KlantID = table.Column<int>(nullable: false),
                    BestellingID = table.Column<int>(nullable: false),
                    FactuurfirmaID = table.Column<int>(nullable: false),
                    BestellingID1 = table.Column<int>(nullable: true),
                    FactuurfirmaID1 = table.Column<int>(nullable: true),
                    KlantID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturen", x => x.FactuurID);
                    table.ForeignKey(
                        name: "FK_Facturen_Bestellingen_BestellingID",
                        column: x => x.BestellingID,
                        principalTable: "Bestellingen",
                        principalColumn: "BestellingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facturen_Bestellingen_BestellingID1",
                        column: x => x.BestellingID1,
                        principalTable: "Bestellingen",
                        principalColumn: "BestellingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facturen_Facturatiefirmas_FactuurfirmaID",
                        column: x => x.FactuurfirmaID,
                        principalTable: "Facturatiefirmas",
                        principalColumn: "FactuurfirmaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facturen_Facturatiefirmas_FactuurfirmaID1",
                        column: x => x.FactuurfirmaID1,
                        principalTable: "Facturatiefirmas",
                        principalColumn: "FactuurfirmaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facturen_Klanten_KlantID",
                        column: x => x.KlantID,
                        principalTable: "Klanten",
                        principalColumn: "KlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facturen_Klanten_KlantID1",
                        column: x => x.KlantID1,
                        principalTable: "Klanten",
                        principalColumn: "KlantID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Factuurlijnen",
                columns: table => new
                {
                    FactuurlijnID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aantal = table.Column<int>(nullable: false),
                    Lijnprijs = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Eenheidsprijs = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    BtwBedrag = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    FactuurID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factuurlijnen", x => x.FactuurlijnID);
                    table.ForeignKey(
                        name: "FK_Factuurlijnen_Facturen_FactuurID",
                        column: x => x.FactuurID,
                        principalTable: "Facturen",
                        principalColumn: "FactuurID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factuurlijnen_Producten_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Producten",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllergeenProducten_AllergeenID",
                table: "AllergeenProducten",
                column: "AllergeenID");

            migrationBuilder.CreateIndex(
                name: "IX_AllergeenProducten_ProductID",
                table: "AllergeenProducten",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellijnen_BestellingID",
                table: "Bestellijnen",
                column: "BestellingID");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellijnen_ProductID",
                table: "Bestellijnen",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_KlantID",
                table: "Bestellingen",
                column: "KlantID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_KlantID1",
                table: "Bestellingen",
                column: "KlantID1");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_LeverAdresID",
                table: "Bestellingen",
                column: "LeverAdresID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_LeverAdresID1",
                table: "Bestellingen",
                column: "LeverAdresID1");

            migrationBuilder.CreateIndex(
                name: "IX_Facturatiefirmas_LandID",
                table: "Facturatiefirmas",
                column: "LandID");

            migrationBuilder.CreateIndex(
                name: "IX_Facturatiefirmas_PostcodeID",
                table: "Facturatiefirmas",
                column: "PostcodeID");

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
                name: "IX_Facturen_KlantID",
                table: "Facturen",
                column: "KlantID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_KlantID1",
                table: "Facturen",
                column: "KlantID1");

            migrationBuilder.CreateIndex(
                name: "IX_Factuurlijnen_FactuurID",
                table: "Factuurlijnen",
                column: "FactuurID");

            migrationBuilder.CreateIndex(
                name: "IX_Factuurlijnen_ProductID",
                table: "Factuurlijnen",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Klanten_LandID",
                table: "Klanten",
                column: "LandID");

            migrationBuilder.CreateIndex(
                name: "IX_Klanten_PostcodeID",
                table: "Klanten",
                column: "PostcodeID");

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_KlantID",
                table: "LeverAdressen",
                column: "KlantID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeverAdressen_KlantID1",
                table: "LeverAdressen",
                column: "KlantID1");

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
                name: "IX_Producten_BtwtypeBtwID",
                table: "Producten",
                column: "BtwtypeBtwID");

            migrationBuilder.CreateIndex(
                name: "IX_Producten_EenheidID",
                table: "Producten",
                column: "EenheidID");

            migrationBuilder.CreateIndex(
                name: "IX_Producten_ProductTypeID",
                table: "Producten",
                column: "ProductTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllergeenProducten");

            migrationBuilder.DropTable(
                name: "Bestellijnen");

            migrationBuilder.DropTable(
                name: "Factuurlijnen");

            migrationBuilder.DropTable(
                name: "Allergenen");

            migrationBuilder.DropTable(
                name: "Facturen");

            migrationBuilder.DropTable(
                name: "Producten");

            migrationBuilder.DropTable(
                name: "Bestellingen");

            migrationBuilder.DropTable(
                name: "Facturatiefirmas");

            migrationBuilder.DropTable(
                name: "BtwTypes");

            migrationBuilder.DropTable(
                name: "Eenheden");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "LeverAdressen");

            migrationBuilder.DropTable(
                name: "Klanten");

            migrationBuilder.DropTable(
                name: "Landen");

            migrationBuilder.DropTable(
                name: "Postcodes");
        }
    }
}
