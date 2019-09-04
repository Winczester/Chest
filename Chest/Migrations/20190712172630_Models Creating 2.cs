using Microsoft.EntityFrameworkCore.Migrations;

namespace Chest.Migrations
{
    public partial class ModelsCreating2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ManufacturerCategories_CategoryID",
                table: "ManufacturerCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ManufacturerCategories",
                table: "ManufacturerCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ManufacturerCategories",
                table: "ManufacturerCategories",
                columns: new[] { "CategoryID", "ManufacturerID" });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerCategories_ManufacturerID",
                table: "ManufacturerCategories",
                column: "ManufacturerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ManufacturerCategories",
                table: "ManufacturerCategories");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturerCategories_ManufacturerID",
                table: "ManufacturerCategories");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ManufacturerCategories_CategoryID",
                table: "ManufacturerCategories",
                column: "CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ManufacturerCategories",
                table: "ManufacturerCategories",
                column: "ManufacturerID");
        }
    }
}
