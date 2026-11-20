using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updentityFileDrive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FolderParentId",
                table: "archivos",
                newName: "carpeta_padre_id");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "archivos",
                newName: "nombre_archivo");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "archivos",
                newName: "id_archivo");

            migrationBuilder.RenameColumn(
                name: "FileExtention",
                table: "archivos",
                newName: "extension");

            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "archivos",
                newName: "tipo_documento");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "archivos",
                newName: "documento_id");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "archivos",
                newName: "tipo_contenido");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tipo_documento",
                table: "archivos",
                newName: "DocumentType");

            migrationBuilder.RenameColumn(
                name: "tipo_contenido",
                table: "archivos",
                newName: "ContentType");

            migrationBuilder.RenameColumn(
                name: "nombre_archivo",
                table: "archivos",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "id_archivo",
                table: "archivos",
                newName: "FileId");

            migrationBuilder.RenameColumn(
                name: "extension",
                table: "archivos",
                newName: "FileExtention");

            migrationBuilder.RenameColumn(
                name: "documento_id",
                table: "archivos",
                newName: "DocumentId");

            migrationBuilder.RenameColumn(
                name: "carpeta_padre_id",
                table: "archivos",
                newName: "FolderParentId");
        }
    }
}
