using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updrenameProductGuidetoMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cupon_material",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nombre_persona = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    proyecto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    presupuesto_id = table.Column<int>(type: "int", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(GETUTCDATE())"),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cupon_material", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "guia_despacho",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    folio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    fecha_despacho = table.Column<DateTime>(type: "datetime2", nullable: true),
                    tipo_documento = table.Column<int>(type: "int", nullable: true),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tipo_moneda = table.Column<int>(type: "int", nullable: true),
                    valor_moneda = table.Column<int>(type: "int", nullable: true),
                    proveedor_id = table.Column<int>(type: "int", nullable: true),
                    cliente_id = table.Column<int>(type: "int", nullable: true),
                    neto = table.Column<int>(type: "int", nullable: true),
                    iva = table.Column<decimal>(type: "decimal(3,2)", precision: 10, scale: 3, nullable: true),
                    valor_total = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(GETUTCDATE())"),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guia_despacho", x => x.id);
                    table.ForeignKey(
                        name: "FK_guia_despacho_ubicacion_LocationId",
                        column: x => x.LocationId,
                        principalTable: "ubicacion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "guia_recepcion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    folio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    fecha_recepcion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    tipo_documento = table.Column<int>(type: "int", nullable: true),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tipo_moneda = table.Column<int>(type: "int", nullable: false),
                    valor_moneda = table.Column<int>(type: "int", nullable: true),
                    proveedor_id = table.Column<int>(type: "int", nullable: true),
                    cliente_id = table.Column<int>(type: "int", nullable: true),
                    neto = table.Column<int>(type: "int", nullable: true),
                    iva = table.Column<decimal>(type: "decimal(3,2)", precision: 10, scale: 3, nullable: true),
                    valor_total = table.Column<int>(type: "int", nullable: true),
                    File = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(GETUTCDATE())"),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guia_recepcion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "material",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    producto_codigo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    unidad_medida = table.Column<int>(type: "int", nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: true),
                    valor_unitario = table.Column<int>(type: "int", nullable: true),
                    valor_total = table.Column<int>(type: "int", nullable: true),
                    tipo_documento = table.Column<int>(type: "int", nullable: true),
                    producto_id = table.Column<int>(type: "int", nullable: true),
                    voucher_id = table.Column<int>(type: "int", nullable: true),
                    recepcion_id = table.Column<int>(type: "int", nullable: true),
                    despacho_id = table.Column<int>(type: "int", nullable: true),
                    DispatchGuideId = table.Column<int>(type: "int", nullable: true),
                    MaterialVoucherId = table.Column<int>(type: "int", nullable: true),
                    ReceptionGuideId = table.Column<int>(type: "int", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(GETUTCDATE())"),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.id);
                    table.ForeignKey(
                        name: "FK_Material_cupon_material_MaterialVoucherId",
                        column: x => x.MaterialVoucherId,
                        principalTable: "cupon_material",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Material_guia_despacho_DispatchGuideId",
                        column: x => x.DispatchGuideId,
                        principalTable: "guia_despacho",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Material_guia_recepcion_ReceptionGuideId",
                        column: x => x.ReceptionGuideId,
                        principalTable: "guia_recepcion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_guia_despacho_LocationId",
                table: "guia_despacho",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_DispatchGuideId",
                table: "Material",
                column: "DispatchGuideId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_MaterialVoucherId",
                table: "Material",
                column: "MaterialVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_ReceptionGuideId",
                table: "Material",
                column: "ReceptionGuideId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "cupon_material");

            migrationBuilder.DropTable(
                name: "guia_despacho");

            migrationBuilder.DropTable(
                name: "guia_recepcion");
        }
    }
}
