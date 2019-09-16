using Microsoft.EntityFrameworkCore.Migrations;

namespace Chest.Migrations
{
    public partial class BaskedGoodsInfoAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoodsName",
                table: "ShopBasket",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GoodsPrice",
                table: "ShopBasket",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoodsName",
                table: "ShopBasket");

            migrationBuilder.DropColumn(
                name: "GoodsPrice",
                table: "ShopBasket");
        }
    }
}
