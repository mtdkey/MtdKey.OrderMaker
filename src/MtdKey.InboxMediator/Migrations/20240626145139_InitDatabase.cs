using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MtdKey.InboxMediator.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "email_mediators",
                columns: table => new
                {
                    EmailAsKey = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Server = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Port = table.Column<int>(type: "int", nullable: false),
                    UseSSL = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_mediators", x => x.EmailAsKey);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inbox_headers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EmailAsKey = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UID = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_headers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inbox_headers_email_mediators_EmailAsKey",
                        column: x => x.EmailAsKey,
                        principalTable: "email_mediators",
                        principalColumn: "EmailAsKey",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "loader_flags",
                columns: table => new
                {
                    EmailAsKey = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Complete = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loader_flags", x => x.EmailAsKey);
                    table.ForeignKey(
                        name: "FK_loader_flags_email_mediators_EmailAsKey",
                        column: x => x.EmailAsKey,
                        principalTable: "email_mediators",
                        principalColumn: "EmailAsKey",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reader_flags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmailAsKey = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReaderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaxUID = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reader_flags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reader_flags_email_mediators_EmailAsKey",
                        column: x => x.EmailAsKey,
                        principalTable: "email_mediators",
                        principalColumn: "EmailAsKey",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_inbox_headers_EmailAsKey_UID",
                table: "inbox_headers",
                columns: new[] { "EmailAsKey", "UID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reader_flags_EmailAsKey_ReaderId",
                table: "reader_flags",
                columns: new[] { "EmailAsKey", "ReaderId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_headers");

            migrationBuilder.DropTable(
                name: "loader_flags");

            migrationBuilder.DropTable(
                name: "reader_flags");

            migrationBuilder.DropTable(
                name: "email_mediators");
        }
    }
}
