using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniAmazon.Migrations
{
    /// <inheritdoc />
    public partial class SecondMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Products",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "CreatedBy", "Description", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, "Admin", "A high-performance laptop", "Laptop", 1500.0, 10 },
                    { 2, "Admin", "A latest model smartphone", "Smartphone", 800.0, 20 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Eamil", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "john@example.com", "John Doe", "password123", "Customer" },
                    { 2, "admin@example.com", "Admin User", "adminpassword", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "OrderDate", "Status", "TotalAmount", "UserID" },
                values: new object[] { 1, new DateTime(2025, 3, 23, 21, 1, 47, 959, DateTimeKind.Local).AddTicks(9025), "Pending", 2300.0, 1 });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemID", "OrderID", "Price", "ProductID", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1500.0, 1, 1 },
                    { 2, 1, 800.0, 2, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedBy",
                table: "Products",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
