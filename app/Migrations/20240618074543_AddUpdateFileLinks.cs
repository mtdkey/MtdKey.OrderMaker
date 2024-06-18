using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MtdKey.OrderMaker.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateFileLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mtd_store_file_links",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Result = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store_file_links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mtd_store_file_links_mtd_form_part_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mtd_store_file_links_mtd_store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FILE_LINK_RESULT",
                table: "mtd_store_file_links",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_file_links_FieldId",
                table: "mtd_store_file_links",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_file_links_StoreId",
                table: "mtd_store_file_links",
                column: "StoreId");


            migrationBuilder.Sql("INSERT INTO mtd_sys_type (id, name, description, active) " +
                   "SELECT 14, 'FileStorage', 'FileStorage',1 " +
                   "WHERE NOT EXISTS (SELECT * FROM mtd_sys_type WHERE id = 14);", true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mtd_store_file_links");

            migrationBuilder.Sql("delete from mtd_sys_type where id=14;", true);
        }
    }
}
