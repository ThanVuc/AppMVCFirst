using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppMVC.Migrations
{
    /// <inheritdoc />
    public partial class FixNumberbecomeQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Products",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Products",
                newName: "Number");
        }
    }
}
