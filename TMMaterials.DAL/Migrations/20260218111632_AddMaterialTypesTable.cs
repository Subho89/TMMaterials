using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMMaterials.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialTypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "tblCollectionStandards");

            migrationBuilder.AddColumn<int>(
                name: "materialTypeId",
                table: "tblCollectionStandards",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblMaterialTypes",
                columns: table => new
                {
                    materialTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMaterialTypes", x => x.materialTypeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblMaterialTypes");

            migrationBuilder.DropColumn(
                name: "materialTypeId",
                table: "tblCollectionStandards");

            migrationBuilder.AddColumn<string>(
                name: "MaterialType",
                table: "tblCollectionStandards",
                type: "TEXT",
                nullable: true);
        }
    }
}
