using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chest.Migrations
{
    public partial class BaskedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopBasketId",
                table: "Goods",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShopBasket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GoodsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopBasket", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goods_ShopBasket_ShopBasketId",
                table: "Goods");

            migrationBuilder.DropTable(
                name: "ShopBasket");

            migrationBuilder.DropIndex(
                name: "IX_Goods_ShopBasketId",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "ShopBasketId",
                table: "Goods");
        }
    }
}
