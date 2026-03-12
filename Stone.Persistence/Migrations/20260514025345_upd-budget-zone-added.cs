using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updbudgetzoneadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zona_id",
                table: "presupuesto");

            migrationBuilder.AlterColumn<int>(
                name: "zona",
                table: "presupuesto",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "zona",
                table: "presupuesto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "zona_id",
                table: "presupuesto",
                type: "int",
                nullable: true);
        }
    }
}
