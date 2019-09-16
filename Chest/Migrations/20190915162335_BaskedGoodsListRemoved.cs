using Microsoft.EntityFrameworkCore.Migrations;

namespace Chest.Migrations
{
    public partial class BaskedGoodsListRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goods_ShopBasket_ShopBasketId",
                table: "Goods");

            migrationBuilder.DropIndex(
                name: "IX_Goods_ShopBasketId",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "ShopBasketId",
                table: "Goods");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopBasketId",
                table: "Goods",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_ShopBasketId",
                table: "Goods",
                column: "ShopBasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goods_ShopBasket_ShopBasketId",
                table: "Goods",
                column: "ShopBasketId",
                principalTable: "ShopBasket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
