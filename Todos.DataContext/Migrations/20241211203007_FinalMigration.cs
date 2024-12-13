using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todos.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Categories_CategoryModelId",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "CategoryModelId",
                table: "Todos",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_CategoryModelId",
                table: "Todos",
                newName: "IX_Todos_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Categories_CategoryId",
                table: "Todos",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Categories_CategoryId",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Todos",
                newName: "CategoryModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_CategoryId",
                table: "Todos",
                newName: "IX_Todos_CategoryModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Categories_CategoryModelId",
                table: "Todos",
                column: "CategoryModelId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
