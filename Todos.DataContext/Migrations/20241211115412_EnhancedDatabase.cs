using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todos.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CategoryModel",
                keyColumn: "Id",
                keyValue: 1,
                column: "Category",
                value: "1");

            migrationBuilder.UpdateData(
                table: "CategoryModel",
                keyColumn: "Id",
                keyValue: 2,
                column: "Category",
                value: "2");

            migrationBuilder.UpdateData(
                table: "CategoryModel",
                keyColumn: "Id",
                keyValue: 3,
                column: "Category",
                value: "3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CategoryModel",
                keyColumn: "Id",
                keyValue: 1,
                column: "Category",
                value: null);

            migrationBuilder.UpdateData(
                table: "CategoryModel",
                keyColumn: "Id",
                keyValue: 2,
                column: "Category",
                value: null);

            migrationBuilder.UpdateData(
                table: "CategoryModel",
                keyColumn: "Id",
                keyValue: 3,
                column: "Category",
                value: null);
        }
    }
}
