using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updcustomerbusinessActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "razon_social",
                table: "cliente");

            migrationBuilder.DropColumn(
                name: "rut",
                table: "cliente");

            migrationBuilder.RenameColumn(
                name: "Giro",
                table: "proveedor",
                newName: "giro");

            migrationBuilder.RenameColumn(
                name: "Giro",
                table: "cliente",
                newName: "giro");

            migrationBuilder.AlterColumn<string>(
                name: "giro",
                table: "cliente",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "giro",
                table: "proveedor",
                newName: "Giro");

            migrationBuilder.RenameColumn(
                name: "giro",
                table: "cliente",
                newName: "Giro");

            migrationBuilder.AlterColumn<string>(
                name: "Giro",
                table: "cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "razon_social",
                table: "cliente",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "rut",
                table: "cliente",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");
        }
    }
}
