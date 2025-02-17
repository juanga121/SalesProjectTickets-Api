using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesProjectTickets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Permi : Migration
    {
        private const string NameColumPermissions = "Permissions";
        private const string NameColumPermissionsOriginal = "Permission";
        private const string NameColumIdTicketOriginal = "Id_ticket";
        private const string NameColumIdTicketNew = "Id";
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: NameColumPermissionsOriginal,
                table: "Users",
                newName: NameColumPermissions);

            migrationBuilder.RenameColumn(
                name: NameColumIdTicketOriginal,
                table: "Tickets",
                newName: NameColumIdTicketNew);

            migrationBuilder.AddColumn<int>(
                name: NameColumPermissions,
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: NameColumPermissions,
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: NameColumPermissions,
                table: "Users",
                newName: NameColumPermissionsOriginal);

            migrationBuilder.RenameColumn(
                name: NameColumIdTicketNew,
                table: "Tickets",
                newName: NameColumIdTicketOriginal);
        }
    }
}
