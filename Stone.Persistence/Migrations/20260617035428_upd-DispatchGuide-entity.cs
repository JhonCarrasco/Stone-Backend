using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updDispatchGuideentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_guia_despacho_ubicacion_LocationId",
                table: "guia_despacho");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "guia_despacho",
                newName: "ubicacion_id");

            migrationBuilder.RenameIndex(
                name: "IX_guia_despacho_LocationId",
                table: "guia_despacho",
                newName: "IX_guia_despacho_ubicacion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_guia_despacho_ubicacion_ubicacion_id",
                table: "guia_despacho",
                column: "ubicacion_id",
                principalTable: "ubicacion",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_guia_despacho_ubicacion_ubicacion_id",
                table: "guia_despacho");

            migrationBuilder.RenameColumn(
                name: "ubicacion_id",
                table: "guia_despacho",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_guia_despacho_ubicacion_id",
                table: "guia_despacho",
                newName: "IX_guia_despacho_LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_guia_despacho_ubicacion_LocationId",
                table: "guia_despacho",
                column: "LocationId",
                principalTable: "ubicacion",
                principalColumn: "id");
        }
    }
}
