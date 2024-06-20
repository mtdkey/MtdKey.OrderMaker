using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MtdKey.OrderMaker.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateHtmlEditor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO mtd_sys_type (id, name, description, active) " +
               "SELECT 15, 'HTMLEditor', 'HTMLEditor',1 " +
               "WHERE NOT EXISTS (SELECT * FROM mtd_sys_type WHERE id = 15);", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from mtd_sys_type where id=15;", true);
        }
    }
}
