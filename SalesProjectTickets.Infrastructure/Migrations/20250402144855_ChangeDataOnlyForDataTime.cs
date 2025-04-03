using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesProjectTickets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDataOnlyForDataTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Creation_date",
                table: "PurchaseHistory",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "Creation_date",
                table: "PurchaseHistory",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
