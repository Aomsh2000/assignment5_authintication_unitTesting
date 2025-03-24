using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniAmazon.Migrations
{
    /// <inheritdoc />
    public partial class JsonMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2025, 3, 23, 21, 37, 43, 848, DateTimeKind.Local).AddTicks(3641));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2025, 3, 23, 21, 35, 10, 917, DateTimeKind.Local).AddTicks(4354));
        }
    }
}
