using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Todos.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedDabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "TodosModel");

            migrationBuilder.AddColumn<int>(
                name: "CategoryModelId",
                table: "TodosModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoryModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryModel", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CategoryModel",
                columns: new[] { "Id", "Category", "Title" },
                values: new object[,]
                {
                    { 1, null, "Beginner" },
                    { 2, null, "Intermediate" },
                    { 3, null, "Experienced" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodosModel_CategoryModelId",
                table: "TodosModel",
                column: "CategoryModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodosModel_CategoryModel_CategoryModelId",
                table: "TodosModel",
                column: "CategoryModelId",
                principalTable: "CategoryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodosModel_CategoryModel_CategoryModelId",
                table: "TodosModel");

            migrationBuilder.DropTable(
                name: "CategoryModel");

            migrationBuilder.DropIndex(
                name: "IX_TodosModel_CategoryModelId",
                table: "TodosModel");

            migrationBuilder.DropColumn(
                name: "CategoryModelId",
                table: "TodosModel");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "TodosModel",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
