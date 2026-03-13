using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competiciones.Migrations
{
    /// <inheritdoc />
    public partial class updateEquipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RutaEscudo",
                table: "Equipos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RutaEscudo",
                table: "Equipos");
        }
    }
}
