using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMMaterials.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCollectionProperties",
                columns: table => new
                {
                    propertyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PropertyName = table.Column<string>(type: "TEXT", nullable: false),
                    DataType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCollectionProperties", x => x.propertyId);
                });

            migrationBuilder.CreateTable(
                name: "tblCollectionPropertiesValues",
                columns: table => new
                {
                    valueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    propertyId = table.Column<int>(type: "INTEGER", nullable: false),
                    collectionStandardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCollectionPropertiesValues", x => x.valueId);
                });

            migrationBuilder.CreateTable(
                name: "tblCollections",
                columns: table => new
                {
                    collectionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CollectionName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCollections", x => x.collectionId);
                });

            migrationBuilder.CreateTable(
                name: "tblCollectionStandards",
                columns: table => new
                {
                    collectionStandardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    collectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    standardId = table.Column<int>(type: "INTEGER", nullable: false),
                    mainId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaterialName = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialType = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialGrade = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCollectionStandards", x => x.collectionStandardId);
                });

            migrationBuilder.CreateTable(
                name: "tblMain",
                columns: table => new
                {
                    mainId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RegionName = table.Column<string>(type: "TEXT", nullable: false),
                    FileID = table.Column<string>(type: "TEXT", nullable: false),
                    LengthUnits = table.Column<string>(type: "TEXT", nullable: false),
                    ForceUnits = table.Column<string>(type: "TEXT", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMain", x => x.mainId);
                });

            migrationBuilder.CreateTable(
                name: "tblMaterialsLibrary",
                columns: table => new
                {
                    libraryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mainId = table.Column<int>(type: "INTEGER", nullable: false),
                    collectionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMaterialsLibrary", x => x.libraryId);
                });

            migrationBuilder.CreateTable(
                name: "tblStandards",
                columns: table => new
                {
                    standardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StandardName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblStandards", x => x.standardId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCollectionProperties_PropertyName",
                table: "tblCollectionProperties",
                column: "PropertyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblMain_RegionName",
                table: "tblMain",
                column: "RegionName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCollectionProperties");

            migrationBuilder.DropTable(
                name: "tblCollectionPropertiesValues");

            migrationBuilder.DropTable(
                name: "tblCollections");

            migrationBuilder.DropTable(
                name: "tblCollectionStandards");

            migrationBuilder.DropTable(
                name: "tblMain");

            migrationBuilder.DropTable(
                name: "tblMaterialsLibrary");

            migrationBuilder.DropTable(
                name: "tblStandards");
        }
    }
}
