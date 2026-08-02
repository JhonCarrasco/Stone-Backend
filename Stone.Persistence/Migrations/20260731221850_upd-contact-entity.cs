using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updcontactentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contacto_persona_persona_id",
                table: "contacto");

            migrationBuilder.DropIndex(
                name: "IX_contacto_persona_id",
                table: "contacto");

            migrationBuilder.DropColumn(
                name: "persona_id",
                table: "contacto");

            migrationBuilder.AddColumn<string>(
                name: "nombre_contacto",
                table: "contacto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nombre_contacto",
                table: "contacto");

            migrationBuilder.AddColumn<int>(
                name: "persona_id",
                table: "contacto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_contacto_persona_id",
                table: "contacto",
                column: "persona_id");

            migrationBuilder.AddForeignKey(
                name: "FK_contacto_persona_persona_id",
                table: "contacto",
                column: "persona_id",
                principalTable: "persona",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
