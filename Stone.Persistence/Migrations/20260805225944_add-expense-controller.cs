using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addexpensecontroller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rendir_gastos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gasto_tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fecha_gasto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    empleado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    proyecto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    presupuesto_id = table.Column<int>(type: "int", nullable: true),
                    archivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    monto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    metodo_tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pago_tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numero_recibo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    vehiculo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    patente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(GETUTCDATE())"),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rendir_gastos", x => x.id);
                    table.ForeignKey(
                        name: "FK_rendir_gastos_presupuesto_presupuesto_id",
                        column: x => x.presupuesto_id,
                        principalTable: "presupuesto",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_rendir_gastos_presupuesto_id",
                table: "rendir_gastos",
                column: "presupuesto_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rendir_gastos");
        }
    }
}
