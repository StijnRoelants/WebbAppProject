using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop_CookInStyle.Migrations
{
    public partial class BtwFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producten_BtwTypes_BtwtypeBtwID",
                table: "Producten");

            migrationBuilder.DropIndex(
                name: "IX_Producten_BtwtypeBtwID",
                table: "Producten");

            migrationBuilder.DropColumn(
                name: "BtwtypeBtwID",
                table: "Producten");

            migrationBuilder.CreateIndex(
                name: "IX_Producten_BtwID",
                table: "Producten",
                column: "BtwID");

            migrationBuilder.AddForeignKey(
                name: "FK_Producten_BtwTypes_BtwID",
                table: "Producten",
                column: "BtwID",
                principalTable: "BtwTypes",
                principalColumn: "BtwID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producten_BtwTypes_BtwID",
                table: "Producten");

            migrationBuilder.DropIndex(
                name: "IX_Producten_BtwID",
                table: "Producten");

            migrationBuilder.AddColumn<int>(
                name: "BtwtypeBtwID",
                table: "Producten",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producten_BtwtypeBtwID",
                table: "Producten",
                column: "BtwtypeBtwID");

            migrationBuilder.AddForeignKey(
                name: "FK_Producten_BtwTypes_BtwtypeBtwID",
                table: "Producten",
                column: "BtwtypeBtwID",
                principalTable: "BtwTypes",
                principalColumn: "BtwID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
