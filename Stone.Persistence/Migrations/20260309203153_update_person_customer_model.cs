using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update_person_customer_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "materno",
                table: "persona");

            migrationBuilder.DropColumn(
                name: "nombre",
                table: "persona");

            migrationBuilder.DropColumn(
                name: "paterno",
                table: "persona");

            migrationBuilder.DropColumn(
                name: "segundo_nombre",
                table: "persona");

            migrationBuilder.RenameColumn(
                name: "fullname",
                table: "cliente",
                newName: "razon_social");

            migrationBuilder.AlterColumn<decimal>(
                name: "largo",
                table: "producto",
                type: "decimal(8,3)",
                precision: 10,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3",
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
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3",
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
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3",
                oldPrecision: 10,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombre_persona",
                table: "persona",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "tipo_persona",
                table: "persona",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nombre_persona",
                table: "persona");

            migrationBuilder.DropColumn(
                name: "tipo_persona",
                table: "persona");

            migrationBuilder.RenameColumn(
                name: "razon_social",
                table: "cliente",
                newName: "fullname");

            migrationBuilder.AlterColumn<decimal>(
                name: "largo",
                table: "producto",
                type: "decimal(8,3",
                precision: 10,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)",
                oldPrecision: 10,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "espesor",
                table: "producto",
                type: "decimal(8,3",
                precision: 10,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)",
                oldPrecision: 10,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ancho",
                table: "producto",
                type: "decimal(8,3",
                precision: 10,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)",
                oldPrecision: 10,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "materno",
                table: "persona",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombre",
                table: "persona",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "paterno",
                table: "persona",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "segundo_nombre",
                table: "persona",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);
        }
    }
}
