using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updrenameProductGuidetoMaterialv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_cupon_material_MaterialVoucherId",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_guia_despacho_DispatchGuideId",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_guia_recepcion_ReceptionGuideId",
                table: "Material");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Material",
                table: "Material");

            migrationBuilder.RenameTable(
                name: "Material",
                newName: "material");

            migrationBuilder.RenameIndex(
                name: "IX_Material_ReceptionGuideId",
                table: "material",
                newName: "IX_material_ReceptionGuideId");

            migrationBuilder.RenameIndex(
                name: "IX_Material_MaterialVoucherId",
                table: "material",
                newName: "IX_material_MaterialVoucherId");

            migrationBuilder.RenameIndex(
                name: "IX_Material_DispatchGuideId",
                table: "material",
                newName: "IX_material_DispatchGuideId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_material",
                table: "material",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_material_cupon_material_MaterialVoucherId",
                table: "material",
                column: "MaterialVoucherId",
                principalTable: "cupon_material",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_material_guia_despacho_DispatchGuideId",
                table: "material",
                column: "DispatchGuideId",
                principalTable: "guia_despacho",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_material_guia_recepcion_ReceptionGuideId",
                table: "material",
                column: "ReceptionGuideId",
                principalTable: "guia_recepcion",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_material_cupon_material_MaterialVoucherId",
                table: "material");

            migrationBuilder.DropForeignKey(
                name: "FK_material_guia_despacho_DispatchGuideId",
                table: "material");

            migrationBuilder.DropForeignKey(
                name: "FK_material_guia_recepcion_ReceptionGuideId",
                table: "material");

            migrationBuilder.DropPrimaryKey(
                name: "PK_material",
                table: "material");

            migrationBuilder.RenameTable(
                name: "material",
                newName: "Material");

            migrationBuilder.RenameIndex(
                name: "IX_material_ReceptionGuideId",
                table: "Material",
                newName: "IX_Material_ReceptionGuideId");

            migrationBuilder.RenameIndex(
                name: "IX_material_MaterialVoucherId",
                table: "Material",
                newName: "IX_Material_MaterialVoucherId");

            migrationBuilder.RenameIndex(
                name: "IX_material_DispatchGuideId",
                table: "Material",
                newName: "IX_Material_DispatchGuideId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Material",
                table: "Material",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_cupon_material_MaterialVoucherId",
                table: "Material",
                column: "MaterialVoucherId",
                principalTable: "cupon_material",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_guia_despacho_DispatchGuideId",
                table: "Material",
                column: "DispatchGuideId",
                principalTable: "guia_despacho",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_guia_recepcion_ReceptionGuideId",
                table: "Material",
                column: "ReceptionGuideId",
                principalTable: "guia_recepcion",
                principalColumn: "id");
        }
    }
}
