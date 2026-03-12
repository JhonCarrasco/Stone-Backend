using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UPDbudgetentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_producto_fabricante_fabricante_id",
                table: "producto");

            migrationBuilder.AlterColumn<int>(
                name: "fabricante_id",
                table: "producto",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "estado",
                table: "presupuesto",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_producto_fabricante_fabricante_id",
                table: "producto",
                column: "fabricante_id",
                principalTable: "fabricante",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_producto_fabricante_fabricante_id",
                table: "producto");

            migrationBuilder.DropColumn(
                name: "estado",
                table: "presupuesto");

            migrationBuilder.AlterColumn<int>(
                name: "fabricante_id",
                table: "producto",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_producto_fabricante_fabricante_id",
                table: "producto",
                column: "fabricante_id",
                principalTable: "fabricante",
                principalColumn: "id");
        }
    }
}
