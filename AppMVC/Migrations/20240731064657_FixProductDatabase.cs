using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppMVC.Migrations
{
    /// <inheritdoc />
    public partial class FixProductDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategoryProducts_Posts_ProductID",
                table: "ProductCategoryProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategoryProducts_Products_ProductModelProductId",
                table: "ProductCategoryProducts");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategoryProducts_ProductModelProductId",
                table: "ProductCategoryProducts");

            migrationBuilder.DropColumn(
                name: "ProductModelProductId",
                table: "ProductCategoryProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategoryProducts_Products_ProductID",
                table: "ProductCategoryProducts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategoryProducts_Products_ProductID",
                table: "ProductCategoryProducts");

            migrationBuilder.AddColumn<int>(
                name: "ProductModelProductId",
                table: "ProductCategoryProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryProducts_ProductModelProductId",
                table: "ProductCategoryProducts",
                column: "ProductModelProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategoryProducts_Posts_ProductID",
                table: "ProductCategoryProducts",
                column: "ProductID",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategoryProducts_Products_ProductModelProductId",
                table: "ProductCategoryProducts",
                column: "ProductModelProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
