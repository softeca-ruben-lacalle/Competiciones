using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competiciones.Migrations
{
    /// <inheritdoc />
    public partial class updateJugador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Altura",
                table: "Jugadores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "Jugadores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastNames",
                table: "Jugadores",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<float>(
                name: "Peso",
                table: "Jugadores",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altura",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "LastNames",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "Peso",
                table: "Jugadores");
        }
    }
}
