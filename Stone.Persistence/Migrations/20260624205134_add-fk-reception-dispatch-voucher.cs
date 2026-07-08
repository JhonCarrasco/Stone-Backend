using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addfkreceptiondispatchvoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_guia_despacho_ubicacion_ubicacion_id",
                table: "guia_despacho");

            migrationBuilder.DropIndex(
                name: "IX_guia_despacho_ubicacion_id",
                table: "guia_despacho");

            migrationBuilder.CreateIndex(
                name: "IX_guia_recepcion_cliente_id",
                table: "guia_recepcion",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_guia_recepcion_proveedor_id",
                table: "guia_recepcion",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_guia_despacho_cliente_id",
                table: "guia_despacho",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_guia_despacho_proveedor_id",
                table: "guia_despacho",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_cupon_material_presupuesto_id",
                table: "cupon_material",
                column: "presupuesto_id");

            migrationBuilder.AddForeignKey(
                name: "FK_cupon_material_presupuesto_presupuesto_id",
                table: "cupon_material",
                column: "presupuesto_id",
                principalTable: "presupuesto",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_guia_despacho_cliente_cliente_id",
                table: "guia_despacho",
                column: "cliente_id",
                principalTable: "cliente",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_guia_despacho_proveedor_proveedor_id",
                table: "guia_despacho",
                column: "proveedor_id",
                principalTable: "proveedor",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_guia_recepcion_cliente_cliente_id",
                table: "guia_recepcion",
                column: "cliente_id",
                principalTable: "cliente",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_guia_recepcion_proveedor_proveedor_id",
                table: "guia_recepcion",
                column: "proveedor_id",
                principalTable: "proveedor",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cupon_material_presupuesto_presupuesto_id",
                table: "cupon_material");

            migrationBuilder.DropForeignKey(
                name: "FK_guia_despacho_cliente_cliente_id",
                table: "guia_despacho");

            migrationBuilder.DropForeignKey(
                name: "FK_guia_despacho_proveedor_proveedor_id",
                table: "guia_despacho");

            migrationBuilder.DropForeignKey(
                name: "FK_guia_recepcion_cliente_cliente_id",
                table: "guia_recepcion");

            migrationBuilder.DropForeignKey(
                name: "FK_guia_recepcion_proveedor_proveedor_id",
                table: "guia_recepcion");

            migrationBuilder.DropIndex(
                name: "IX_guia_recepcion_cliente_id",
                table: "guia_recepcion");

            migrationBuilder.DropIndex(
                name: "IX_guia_recepcion_proveedor_id",
                table: "guia_recepcion");

            migrationBuilder.DropIndex(
                name: "IX_guia_despacho_cliente_id",
                table: "guia_despacho");

            migrationBuilder.DropIndex(
                name: "IX_guia_despacho_proveedor_id",
                table: "guia_despacho");

            migrationBuilder.DropIndex(
                name: "IX_cupon_material_presupuesto_id",
                table: "cupon_material");

            migrationBuilder.CreateIndex(
                name: "IX_guia_despacho_ubicacion_id",
                table: "guia_despacho",
                column: "ubicacion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_guia_despacho_ubicacion_ubicacion_id",
                table: "guia_despacho",
                column: "ubicacion_id",
                principalTable: "ubicacion",
                principalColumn: "id");
        }
    }
}
