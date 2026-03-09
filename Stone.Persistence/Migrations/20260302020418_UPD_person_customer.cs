using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UPD_person_customer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cliente_cuenta_banco_cuenta_banco_id",
                table: "cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_cliente_persona_persona_id",
                table: "cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_cliente_ubicacion_ubicacion_id",
                table: "cliente");

            migrationBuilder.AlterColumn<string>(
                name: "segundo_nombre",
                table: "persona",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "materno",
                table: "persona",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "ubicacion_id",
                table: "cliente",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "persona_id",
                table: "cliente",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "cuenta_banco_id",
                table: "cliente",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_cliente_cuenta_banco_cuenta_banco_id",
                table: "cliente",
                column: "cuenta_banco_id",
                principalTable: "cuenta_banco",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_cliente_persona_persona_id",
                table: "cliente",
                column: "persona_id",
                principalTable: "persona",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_cliente_ubicacion_ubicacion_id",
                table: "cliente",
                column: "ubicacion_id",
                principalTable: "ubicacion",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cliente_cuenta_banco_cuenta_banco_id",
                table: "cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_cliente_persona_persona_id",
                table: "cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_cliente_ubicacion_ubicacion_id",
                table: "cliente");

            migrationBuilder.AlterColumn<string>(
                name: "segundo_nombre",
                table: "persona",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "materno",
                table: "persona",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ubicacion_id",
                table: "cliente",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "persona_id",
                table: "cliente",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "cuenta_banco_id",
                table: "cliente",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_cliente_cuenta_banco_cuenta_banco_id",
                table: "cliente",
                column: "cuenta_banco_id",
                principalTable: "cuenta_banco",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cliente_persona_persona_id",
                table: "cliente",
                column: "persona_id",
                principalTable: "persona",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cliente_ubicacion_ubicacion_id",
                table: "cliente",
                column: "ubicacion_id",
                principalTable: "ubicacion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
