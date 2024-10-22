using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MtdKey.OrderMaker.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateEmailReader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "target_forms",
                columns: table => new
                {
                    FormId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_target_forms", x => x.FormId);
                    table.ForeignKey(
                        name: "FK_target_forms_mtd_form_FormId",
                        column: x => x.FormId,
                        principalTable: "mtd_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "target_fields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Target = table.Column<int>(type: "int", nullable: false),
                    FieldId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_target_fields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_target_fields_mtd_form_part_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_target_fields_target_forms_FormId",
                        column: x => x.FormId,
                        principalTable: "target_forms",
                        principalColumn: "FormId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "target_filters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailAsKey = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Filter = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_target_filters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_target_filters_target_forms_FormId",
                        column: x => x.FormId,
                        principalTable: "target_forms",
                        principalColumn: "FormId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_target_fields_FieldId",
                table: "target_fields",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_target_fields_FormId",
                table: "target_fields",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_target_filters_FormId",
                table: "target_filters",
                column: "FormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "target_fields");

            migrationBuilder.DropTable(
                name: "target_filters");

            migrationBuilder.DropTable(
                name: "target_forms");
        }
    }
}
