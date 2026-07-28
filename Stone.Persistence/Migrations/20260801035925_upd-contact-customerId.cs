using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updcontactcustomerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contacto_cliente_cliente_id",
                table: "contacto");

            migrationBuilder.DropForeignKey(
                name: "FK_proveedor_cuenta_banco_cuenta_banco_id",
                table: "proveedor");

            migrationBuilder.DropForeignKey(
                name: "FK_proveedor_persona_persona_id",
                table: "proveedor");

            migrationBuilder.DropForeignKey(
                name: "FK_proveedor_ubicacion_ubicacion_id",
                table: "proveedor");

            migrationBuilder.AlterColumn<int>(
                name: "ubicacion_id",
                table: "proveedor",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "persona_id",
                table: "proveedor",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "giro",
                table: "proveedor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "cuenta_banco_id",
                table: "proveedor",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "cliente_id",
                table: "contacto",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_contacto_cliente_cliente_id",
                table: "contacto",
                column: "cliente_id",
                principalTable: "cliente",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_proveedor_cuenta_banco_cuenta_banco_id",
                table: "proveedor",
                column: "cuenta_banco_id",
                principalTable: "cuenta_banco",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_proveedor_persona_persona_id",
                table: "proveedor",
                column: "persona_id",
                principalTable: "persona",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_proveedor_ubicacion_ubicacion_id",
                table: "proveedor",
                column: "ubicacion_id",
                principalTable: "ubicacion",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contacto_cliente_cliente_id",
                table: "contacto");

            migrationBuilder.DropForeignKey(
                name: "FK_proveedor_cuenta_banco_cuenta_banco_id",
                table: "proveedor");

            migrationBuilder.DropForeignKey(
                name: "FK_proveedor_persona_persona_id",
                table: "proveedor");

            migrationBuilder.DropForeignKey(
                name: "FK_proveedor_ubicacion_ubicacion_id",
                table: "proveedor");

            migrationBuilder.AlterColumn<int>(
                name: "ubicacion_id",
                table: "proveedor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "persona_id",
                table: "proveedor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "giro",
                table: "proveedor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "cuenta_banco_id",
                table: "proveedor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "cliente_id",
                table: "contacto",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_contacto_cliente_cliente_id",
                table: "contacto",
                column: "cliente_id",
                principalTable: "cliente",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_proveedor_cuenta_banco_cuenta_banco_id",
                table: "proveedor",
                column: "cuenta_banco_id",
                principalTable: "cuenta_banco",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_proveedor_persona_persona_id",
                table: "proveedor",
                column: "persona_id",
                principalTable: "persona",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_proveedor_ubicacion_ubicacion_id",
                table: "proveedor",
                column: "ubicacion_id",
                principalTable: "ubicacion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
