using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniAmazon.Migrations
{
    /// <inheritdoc />
    public partial class AuthMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Eamil",
                table: "Users",
                newName: "Email");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2025, 3, 24, 1, 17, 36, 330, DateTimeKind.Local).AddTicks(7636));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Eamil");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2025, 3, 23, 21, 37, 43, 848, DateTimeKind.Local).AddTicks(3641));
        }
    }
}
