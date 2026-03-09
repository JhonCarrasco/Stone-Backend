using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_contact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contacto_persona_PersonId",
                table: "contacto");

            migrationBuilder.DropIndex(
                name: "IX_contacto_PersonId",
                table: "contacto");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "contacto");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contacto_persona_persona_id",
                table: "contacto");

            migrationBuilder.DropIndex(
                name: "IX_contacto_persona_id",
                table: "contacto");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "contacto",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_contacto_PersonId",
                table: "contacto",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_contacto_persona_PersonId",
                table: "contacto",
                column: "PersonId",
                principalTable: "persona",
                principalColumn: "id");
        }
    }
}
