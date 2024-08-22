using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppMVC.Migrations
{
    /// <inheritdoc />
    public partial class Fix1ProductDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategoryProducts_Categories_CategoryID",
                table: "ProductCategoryProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategoryProducts_CategoryProducts_CategoryID",
                table: "ProductCategoryProducts",
                column: "CategoryID",
                principalTable: "CategoryProducts",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategoryProducts_CategoryProducts_CategoryID",
                table: "ProductCategoryProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategoryProducts_Categories_CategoryID",
                table: "ProductCategoryProducts",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
