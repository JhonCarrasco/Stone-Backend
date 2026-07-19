using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class upddispatchcurrencyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ubicacion_id",
                table: "guia_despacho",
                newName: "zona");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_moneda",
                table: "guia_recepcion",
                type: "decimal(3,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tipo_moneda",
                table: "guia_recepcion",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_moneda",
                table: "guia_despacho",
                type: "decimal(3,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tipo_moneda",
                table: "guia_despacho",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "comuna_id",
                table: "guia_despacho",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "direccion",
                table: "guia_despacho",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comuna_id",
                table: "guia_despacho");

            migrationBuilder.DropColumn(
                name: "direccion",
                table: "guia_despacho");

            migrationBuilder.RenameColumn(
                name: "zona",
                table: "guia_despacho",
                newName: "ubicacion_id");

            migrationBuilder.AlterColumn<int>(
                name: "valor_moneda",
                table: "guia_recepcion",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_moneda",
                table: "guia_recepcion",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "valor_moneda",
                table: "guia_despacho",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_moneda",
                table: "guia_despacho",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
