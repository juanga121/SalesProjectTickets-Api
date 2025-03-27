using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesProjectTickets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeForeingKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseHistory_Tickets_TicketsId",
                table: "PurchaseHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseHistory_Users_UsersId",
                table: "PurchaseHistory");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseHistory_TicketsId",
                table: "PurchaseHistory");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseHistory_UsersId",
                table: "PurchaseHistory");

            migrationBuilder.DropColumn(
                name: "TicketsId",
                table: "PurchaseHistory");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "PurchaseHistory");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistory_PaymentsId",
                table: "PurchaseHistory",
                column: "PaymentsId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistory_PaymentsUsersId",
                table: "PurchaseHistory",
                column: "PaymentsUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseHistory_Tickets_PaymentsId",
                table: "PurchaseHistory",
                column: "PaymentsId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseHistory_Users_PaymentsUsersId",
                table: "PurchaseHistory",
                column: "PaymentsUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseHistory_Tickets_PaymentsId",
                table: "PurchaseHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseHistory_Users_PaymentsUsersId",
                table: "PurchaseHistory");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseHistory_PaymentsId",
                table: "PurchaseHistory");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseHistory_PaymentsUsersId",
                table: "PurchaseHistory");

            migrationBuilder.AddColumn<Guid>(
                name: "TicketsId",
                table: "PurchaseHistory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UsersId",
                table: "PurchaseHistory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistory_TicketsId",
                table: "PurchaseHistory",
                column: "TicketsId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistory_UsersId",
                table: "PurchaseHistory",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseHistory_Tickets_TicketsId",
                table: "PurchaseHistory",
                column: "TicketsId",
                principalTable: "Tickets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseHistory_Users_UsersId",
                table: "PurchaseHistory",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
