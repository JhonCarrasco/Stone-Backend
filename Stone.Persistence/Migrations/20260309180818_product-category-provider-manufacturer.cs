using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class productcategoryprovidermanufacturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "producto",
                newName: "descripcion");

            migrationBuilder.AlterColumn<decimal>(
                name: "largo",
                table: "producto",
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
                name: "espesor",
                table: "producto",
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
                table: "producto",
                type: "decimal(8,3)",
                precision: 10,
                scale: 3,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(10)",
                oldPrecision: 10,
                oldScale: 3,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "descripcion",
                table: "producto",
                newName: "Description");

            migrationBuilder.AlterColumn<double>(
                name: "largo",
                table: "producto",
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
                name: "espesor",
                table: "producto",
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
                table: "producto",
                type: "float(10)",
                precision: 10,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)",
                oldPrecision: 10,
                oldScale: 3,
                oldNullable: true);
        }
    }
}
