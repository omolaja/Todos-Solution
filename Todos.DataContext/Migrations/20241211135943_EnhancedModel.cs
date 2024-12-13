using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todos.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodosModel");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "CategoryModel",
                newName: "category");

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TodoId = table.Column<int>(type: "int", nullable: false),
                    Todo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Todos_CategoryModel_CategoryModelId",
                        column: x => x.CategoryModelId,
                        principalTable: "CategoryModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_CategoryModelId",
                table: "Todos",
                column: "CategoryModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "CategoryModel",
                newName: "Category");

            migrationBuilder.CreateTable(
                name: "TodosModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryModelId = table.Column<int>(type: "int", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Todo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TodoId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodosModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodosModel_CategoryModel_CategoryModelId",
                        column: x => x.CategoryModelId,
                        principalTable: "CategoryModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodosModel_CategoryModelId",
                table: "TodosModel",
                column: "CategoryModelId");
        }
    }
}
