using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competiciones.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePartidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipoPartido");

            migrationBuilder.AlterColumn<string>(
                name: "Resultado",
                table: "Partidos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "EquipoLocalId",
                table: "Partidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EquipoVisitanteId",
                table: "Partidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_EquipoLocalId",
                table: "Partidos",
                column: "EquipoLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_EquipoVisitanteId",
                table: "Partidos",
                column: "EquipoVisitanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partidos_Equipos_EquipoLocalId",
                table: "Partidos",
                column: "EquipoLocalId",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partidos_Equipos_EquipoVisitanteId",
                table: "Partidos",
                column: "EquipoVisitanteId",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partidos_Equipos_EquipoLocalId",
                table: "Partidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Partidos_Equipos_EquipoVisitanteId",
                table: "Partidos");

            migrationBuilder.DropIndex(
                name: "IX_Partidos_EquipoLocalId",
                table: "Partidos");

            migrationBuilder.DropIndex(
                name: "IX_Partidos_EquipoVisitanteId",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "EquipoLocalId",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "EquipoVisitanteId",
                table: "Partidos");

            migrationBuilder.UpdateData(
                table: "Partidos",
                keyColumn: "Resultado",
                keyValue: null,
                column: "Resultado",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Resultado",
                table: "Partidos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
        }
    }
}
