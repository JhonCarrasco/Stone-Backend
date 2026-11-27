using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updcommuneentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "region_id",
                table: "comuna",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "region_id",
                table: "comuna");
        }
    }
}
