using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competiciones.Migrations
{
    /// <inheritdoc />
    public partial class AddPartidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Horario = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Resultado = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompeticionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partidos_Competiciones_CompeticionId",
                        column: x => x.CompeticionId,
                        principalTable: "Competiciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipoPartido",
                columns: table => new
                {
                    EquiposId = table.Column<int>(type: "int", nullable: false),
                    PartidosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipoPartido", x => new { x.EquiposId, x.PartidosId });
                    table.ForeignKey(
                        name: "FK_EquipoPartido_Equipos_EquiposId",
                        column: x => x.EquiposId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipoPartido_Partidos_PartidosId",
                        column: x => x.PartidosId,
                        principalTable: "Partidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EquipoPartido_PartidosId",
                table: "EquipoPartido",
                column: "PartidosId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_CompeticionId",
                table: "Partidos",
                column: "CompeticionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipoPartido");

            migrationBuilder.DropTable(
                name: "Partidos");
        }
    }
}
