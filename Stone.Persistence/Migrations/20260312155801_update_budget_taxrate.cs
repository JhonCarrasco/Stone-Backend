using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update_budget_taxrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cliente_nombre",
                table: "presupuesto");

            migrationBuilder.AlterColumn<decimal>(
                name: "iva",
                table: "presupuesto",
                type: "decimal(3,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "largo",
                table: "itemizado_producto",
                type: "decimal(8,3)",
                precision: 10,
                scale: 3,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(10)",
                oldPrecision: 10,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "espesor",
                table: "itemizado_producto",
                type: "decimal(8,3)",
                precision: 10,
                scale: 3,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(10)",
                oldPrecision: 10,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ancho",
                table: "itemizado_producto",
                type: "decimal(8,3)",
                precision: 10,
                scale: 3,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(10)",
                oldPrecision: 10,
                oldScale: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "iva",
                table: "presupuesto",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)");

            migrationBuilder.AddColumn<string>(
                name: "cliente_nombre",
                table: "presupuesto",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "largo",
                table: "itemizado_producto",
                type: "float(10)",
                precision: 10,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)",
                oldPrecision: 10,
                oldScale: 3);

            migrationBuilder.AlterColumn<double>(
                name: "espesor",
                table: "itemizado_producto",
                type: "float(10)",
                precision: 10,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)",
                oldPrecision: 10,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ancho",
                table: "itemizado_producto",
                type: "float(10)",
                precision: 10,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)",
                oldPrecision: 10,
                oldScale: 3);
        }
    }
}
