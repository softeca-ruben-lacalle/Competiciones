using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competiciones.Migrations
{
    /// <inheritdoc />
    public partial class updateCompeticion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pais",
                table: "Competiciones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pais",
                table: "Competiciones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
