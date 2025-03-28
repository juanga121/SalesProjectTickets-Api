using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesProjectTickets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeQuantityHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "PurchaseHistory",
                newName: "QuantityHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityHistory",
                table: "PurchaseHistory",
                newName: "Quantity");
        }
    }
}
