using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MtdKey.OrderMaker.Migrations
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
                name: "mtd_category_form",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    parent = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_category_form", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_config_file",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_size = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_type = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_data = table.Column<byte[]>(type: "mediumblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_config_file", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_config_param",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_config_param", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_group",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_group", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_policy",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_policy", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_sys_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    active = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_sys_style", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_sys_term",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sign = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_sys_term", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_sys_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    active = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_sys_type", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_form",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    active = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    mtd_category = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sequence = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'"),
                    visible_number = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    visible_date = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_form", x => x.id);
                    table.ForeignKey(
                        name: "fk_form_category",
                        column: x => x.mtd_category,
                        principalTable: "mtd_category_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_approval",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_form = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_start = table.Column<byte[]>(type: "mediumblob", nullable: true),
                    img_start_type = table.Column<string>(type: "varchar(48)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_start_text = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_iteraction = table.Column<byte[]>(type: "mediumblob", nullable: true),
                    img_iteraction_type = table.Column<string>(type: "varchar(48)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_iteraction_text = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_waiting = table.Column<byte[]>(type: "mediumblob", nullable: true),
                    img_waiting_type = table.Column<string>(type: "varchar(48)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_waiting_text = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_approved = table.Column<byte[]>(type: "mediumblob", nullable: true),
                    img_approved_type = table.Column<string>(type: "varchar(48)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_approved_text = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_rejected = table.Column<byte[]>(type: "mediumblob", nullable: true),
                    img_rejected_type = table.Column<string>(type: "varchar(48)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_rejected_text = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_required = table.Column<byte[]>(type: "mediumblob", nullable: true),
                    img_required_type = table.Column<string>(type: "varchar(48)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_required_text = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_approval", x => x.id);
                    table.ForeignKey(
                        name: "fk_approvel_form",
                        column: x => x.mtd_form,
                        principalTable: "mtd_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_event_subscribe",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_form_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    event_create = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    event_edit = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_event_subscribe", x => x.id);
                    table.ForeignKey(
                        name: "fk_mtd_event_mtd_form",
                        column: x => x.mtd_form_id,
                        principalTable: "mtd_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_filter",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idUser = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_form = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    page_size = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'10'"),
                    searchText = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    searchNumber = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    page = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'1'"),
                    show_number = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    show_date = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    sort = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sort_order = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_filter", x => x.id);
                    table.ForeignKey(
                        name: "mtd_filter_mtd_form",
                        column: x => x.mtd_form,
                        principalTable: "mtd_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_filter_script",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtd_form_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    script = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_filter_script", x => x.id);
                    table.ForeignKey(
                        name: "fk_script_filter",
                        column: x => x.mtd_form_id,
                        principalTable: "mtd_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_form_desk",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<byte[]>(type: "mediumblob", nullable: false),
                    image_type = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image_size = table.Column<int>(type: "int(11)", nullable: false),
                    color_font = table.Column<string>(type: "varchar(45)", nullable: false, defaultValueSql: "'white'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    color_back = table.Column<string>(type: "varchar(45)", nullable: false, defaultValueSql: "'gray'")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_form_desk", x => x.id);
                    table.ForeignKey(
                        name: "fk_mtd_form_des_mtd_from",
                        column: x => x.id,
                        principalTable: "mtd_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_form_header",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<byte[]>(type: "mediumblob", nullable: false),
                    image_type = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image_size = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_form_header", x => x.id);
                    table.ForeignKey(
                        name: "fk_image_form",
                        column: x => x.id,
                        principalTable: "mtd_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_form_part",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sequence = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'"),
                    active = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    mtd_sys_style = table.Column<int>(type: "int(11)", nullable: false),
                    mtd_form = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_form_part", x => x.id);
                    table.ForeignKey(
                        name: "fk_mtd_form_part_mtd_form1",
                        column: x => x.mtd_form,
                        principalTable: "mtd_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mtd_form_part_mtd_sys_style1",
                        column: x => x.mtd_sys_style,
                        principalTable: "mtd_sys_style",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_policy_forms",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtd_policy = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_form = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    edit_all = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    edit_group = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    edit_own = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    view_all = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    view_group = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    view_own = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    delete_all = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    delete_group = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    delete_own = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    change_owner = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    reviewer = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    change_date = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    own_deny_group = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    export_to_excel = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    related_create = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    related_edit = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_policy_forms", x => x.id);
                    table.ForeignKey(
                        name: "fk_policy_forms_form",
                        column: x => x.mtd_form,
                        principalTable: "mtd_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_policy_forms_policy",
                        column: x => x.mtd_policy,
                        principalTable: "mtd_policy",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_store",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sequence = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'"),
                    active = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    mtd_form = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    timecr = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store", x => x.id);
                    table.ForeignKey(
                        name: "fk_mtd_store_mtd_form1",
                        column: x => x.mtd_form,
                        principalTable: "mtd_form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_approval_stage",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_approval = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stage = table.Column<int>(type: "int(11)", nullable: false),
                    user_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    block_parts = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_approval_stage", x => x.id);
                    table.ForeignKey(
                        name: "fk_stage_approval",
                        column: x => x.mtd_approval,
                        principalTable: "mtd_approval",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_filter_date",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    date_start = table.Column<DateTime>(type: "datetime", nullable: false),
                    date_end = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_filter_date", x => x.id);
                    table.ForeignKey(
                        name: "fk_date_filter",
                        column: x => x.id,
                        principalTable: "mtd_filter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_filter_owner",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    owner_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_filter_owner", x => x.id);
                    table.ForeignKey(
                        name: "fk_owner_filter",
                        column: x => x.id,
                        principalTable: "mtd_filter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_filter_script_apply",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_filter_id = table.Column<int>(type: "int(11)", nullable: false),
                    mtd_filter_script_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_filter_script_apply", x => x.id);
                    table.ForeignKey(
                        name: "fk_script_filter_apply1",
                        column: x => x.mtd_filter_id,
                        principalTable: "mtd_filter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_script_filter_apply2",
                        column: x => x.mtd_filter_script_id,
                        principalTable: "mtd_filter_script",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_policy_scripts",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_policy_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_filter_script_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_policy_scripts", x => x.id);
                    table.ForeignKey(
                        name: "fk_policy_filter",
                        column: x => x.mtd_filter_script_id,
                        principalTable: "mtd_filter_script",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_policy_script",
                        column: x => x.mtd_policy_id,
                        principalTable: "mtd_policy",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_form_part_field",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    required = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    sequence = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'"),
                    active = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    read_only = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    mtd_sys_type = table.Column<int>(type: "int(11)", nullable: false),
                    mtd_form_part = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    default_data = table.Column<string>(type: "varchar(255)", nullable: false, defaultValue: "")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_form_part_field", x => x.id);
                    table.ForeignKey(
                        name: "fk_mtd_form_part_field_mtd_form_part1",
                        column: x => x.mtd_form_part,
                        principalTable: "mtd_form_part",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mtd_form_part_field_mtd_sys_type1",
                        column: x => x.mtd_sys_type,
                        principalTable: "mtd_sys_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_form_part_header",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<byte[]>(type: "mediumblob", nullable: false),
                    image_type = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image_size = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_form_part_header", x => x.id);
                    table.ForeignKey(
                        name: "fk_image_form_part",
                        column: x => x.id,
                        principalTable: "mtd_form_part",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_policy_parts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtd_policy = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_form_part = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    edit = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    view = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_policy_parts", x => x.id);
                    table.ForeignKey(
                        name: "fk_policy_part_part",
                        column: x => x.mtd_form_part,
                        principalTable: "mtd_form_part",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_policy_part_policy",
                        column: x => x.mtd_policy,
                        principalTable: "mtd_policy",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_log_document",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtd_store = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_name = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    timech = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_log_document", x => x.id);
                    table.ForeignKey(
                        name: "fk_log_document_store",
                        column: x => x.mtd_store,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_store_owner",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_name = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store_owner", x => x.id);
                    table.ForeignKey(
                        name: "fk_owner_store",
                        column: x => x.id,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_approval_rejection",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    note = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sequence = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'"),
                    color = table.Column<string>(type: "varchar(45)", nullable: false, defaultValueSql: "'green'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_approval_stage_id = table.Column<int>(type: "int(11)", nullable: false),
                    img_data = table.Column<byte[]>(type: "mediumblob", nullable: true),
                    img_type = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_approval_rejection", x => x.id);
                    table.ForeignKey(
                        name: "fk_rejection_stage",
                        column: x => x.mtd_approval_stage_id,
                        principalTable: "mtd_approval_stage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_approval_resolution",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    note = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sequence = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'"),
                    color = table.Column<string>(type: "varchar(45)", nullable: false, defaultValueSql: "'green'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_approval_stage_id = table.Column<int>(type: "int(11)", nullable: false),
                    img_data = table.Column<byte[]>(type: "mediumblob", nullable: true),
                    img_type = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_approval_resolution", x => x.id);
                    table.ForeignKey(
                        name: "fk_resolution_stage",
                        column: x => x.mtd_approval_stage_id,
                        principalTable: "mtd_approval_stage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_log_approval",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtd_store = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stage = table.Column<int>(type: "int(11)", nullable: false),
                    user_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_name = table.Column<string>(type: "varchar(255)", nullable: false, defaultValueSql: "'No Name'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_recipient_id = table.Column<string>(type: "varchar(36)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_recipient_name = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    result = table.Column<int>(type: "int(11)", nullable: false),
                    timecr = table.Column<DateTime>(type: "datetime", nullable: false),
                    img_data = table.Column<byte[]>(type: "mediumblob", nullable: true),
                    img_type = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    color = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    note = table.Column<string>(type: "varchar(512)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    app_comment = table.Column<string>(type: "varchar(512)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_sign = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_log_approval", x => x.id);
                    table.ForeignKey(
                        name: "fk_log_approval_stage",
                        column: x => x.stage,
                        principalTable: "mtd_approval_stage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_log_approval_store",
                        column: x => x.mtd_store,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_store_approval",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    md_approve_stage = table.Column<int>(type: "int(11)", nullable: false),
                    parts_approved = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    complete = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'"),
                    result = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'"),
                    sign_chain = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_event_time = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store_approval", x => x.id);
                    table.ForeignKey(
                        name: "fk_store_approve",
                        column: x => x.id,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_store_approve_stage",
                        column: x => x.md_approve_stage,
                        principalTable: "mtd_approval_stage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_filter_column",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtd_filter = table.Column<int>(type: "int(11)", nullable: false),
                    mtd_form_part_field = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sequence = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_filter_column", x => x.id);
                    table.ForeignKey(
                        name: "mtd_filter_column_mtd_field",
                        column: x => x.mtd_filter,
                        principalTable: "mtd_filter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "mtd_roster_field",
                        column: x => x.mtd_form_part_field,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_filter_field",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtd_filter = table.Column<int>(type: "int(11)", nullable: false),
                    mtd_form_part_field = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    value = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    value_extra = table.Column<string>(type: "varchar(256)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtd_term = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_filter_field", x => x.id);
                    table.ForeignKey(
                        name: "mtd_filter_field_mtd_field",
                        column: x => x.mtd_filter,
                        principalTable: "mtd_filter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "mtd_filter_field_mtd_form_field",
                        column: x => x.mtd_form_part_field,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "mtd_filter_field_mtd_term",
                        column: x => x.mtd_term,
                        principalTable: "mtd_sys_term",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_form_part_field_item",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldId = table.Column<string>(type: "varchar(36)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_form_part_field_item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mtd_form_part_field_item_mtd_form_part_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_store_date",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Result = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store_date", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mtd_store_date_mtd_form_part_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mtd_store_date_mtd_store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_store_decimal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Result = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store_decimal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mtd_store_decimal_mtd_form_part_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mtd_store_decimal_mtd_store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_store_file",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileName = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Result = table.Column<byte[]>(type: "longblob", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store_file", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mtd_store_file_mtd_form_part_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mtd_store_file_mtd_store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_store_int",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Result = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store_int", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mtd_store_int_mtd_form_part_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mtd_store_int_mtd_store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_store_memo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Result = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store_memo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mtd_store_memo_mtd_form_part_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mtd_store_memo_mtd_store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_store_text",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Result = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store_text", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mtd_store_text_mtd_form_part_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mtd_store_text_mtd_store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mtd_store_item",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Result = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtd_store_item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mtd_store_item_mtd_form_part_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "mtd_form_part_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mtd_store_item_mtd_form_part_field_item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "mtd_form_part_field_item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mtd_store_item_mtd_store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "mtd_store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "fk_approvel_form_idx",
                table: "mtd_approval",
                column: "mtd_form");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_approval",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_rejection_stage_idx",
                table: "mtd_approval_rejection",
                column: "mtd_approval_stage_id");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_approval_rejection",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sequence",
                table: "mtd_approval_rejection",
                column: "sequence");

            migrationBuilder.CreateIndex(
                name: "fk_resolution_stage_idx",
                table: "mtd_approval_resolution",
                column: "mtd_approval_stage_id");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_approval_resolution",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sequence",
                table: "mtd_approval_resolution",
                column: "sequence");

            migrationBuilder.CreateIndex(
                name: "fk_stage_approval_idx",
                table: "mtd_approval_stage",
                column: "mtd_approval");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_approval_stage",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER",
                table: "mtd_approval_stage",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "fk_group_themself_idx",
                table: "mtd_category_form",
                column: "parent");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_category_form",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_config_file",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_config_param",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_mtd_event_mtd_form_idx",
                table: "mtd_event_subscribe",
                column: "mtd_form_id");

            migrationBuilder.CreateIndex(
                name: "id_unique",
                table: "mtd_event_subscribe",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_id",
                table: "mtd_event_subscribe",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_filter",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_INDEX_USER",
                table: "mtd_filter",
                column: "idUser");

            migrationBuilder.CreateIndex(
                name: "mtd_filter_mtd_form_idx",
                table: "mtd_filter",
                column: "mtd_form");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_filter_column",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "mtd_filter_column_idx",
                table: "mtd_filter_column",
                column: "mtd_filter");

            migrationBuilder.CreateIndex(
                name: "mtd_roster_field_idx",
                table: "mtd_filter_column",
                column: "mtd_form_part_field");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_filter_date",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_filter_field",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "mtd_filter_field_mtd_form_field_idx",
                table: "mtd_filter_field",
                column: "mtd_form_part_field");

            migrationBuilder.CreateIndex(
                name: "mtd_filter_field_term_idx",
                table: "mtd_filter_field",
                column: "mtd_term");

            migrationBuilder.CreateIndex(
                name: "mtd_filter_idx",
                table: "mtd_filter_field",
                column: "mtd_filter");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_filter_owner",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_script_filter_idx",
                table: "mtd_filter_script",
                column: "mtd_form_id");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_filter_script",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_script_filter_apply1_idx",
                table: "mtd_filter_script_apply",
                column: "mtd_filter_id");

            migrationBuilder.CreateIndex(
                name: "fk_script_filter_apply2_idx",
                table: "mtd_filter_script_apply",
                column: "mtd_filter_script_id");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_filter_script_apply",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_form",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mtd_form_mtd_category",
                table: "mtd_form",
                column: "mtd_category");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_form_desk",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_form_header",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_mtd_form_part_mtd_form1_idx",
                table: "mtd_form_part",
                column: "mtd_form");

            migrationBuilder.CreateIndex(
                name: "fk_mtd_form_part_mtd_sys_style1_idx",
                table: "mtd_form_part",
                column: "mtd_sys_style");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_form_part",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_mtd_form_part_field_mtd_form_part1_idx",
                table: "mtd_form_part_field",
                column: "mtd_form_part");

            migrationBuilder.CreateIndex(
                name: "fk_mtd_form_part_field_mtd_sys_type1_idx",
                table: "mtd_form_part_field",
                column: "mtd_sys_type");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_form_part_field",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mtd_form_part_field_item_FieldId",
                table: "mtd_form_part_field_item",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_form_part_header",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_group",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_log_approval_stage_idx",
                table: "mtd_log_approval",
                column: "stage");

            migrationBuilder.CreateIndex(
                name: "fk_log_approval_store_idx",
                table: "mtd_log_approval",
                column: "mtd_store");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_log_approval",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_log_document_store_idx",
                table: "mtd_log_document",
                column: "mtd_store");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_log_document",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_date",
                table: "mtd_log_document",
                column: "timech");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_policy",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_policy_forms_form_idx",
                table: "mtd_policy_forms",
                column: "mtd_form");

            migrationBuilder.CreateIndex(
                name: "fk_policy_forms_policy_idx",
                table: "mtd_policy_forms",
                column: "mtd_policy");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_policy_forms",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UNIQUE_FORM",
                table: "mtd_policy_forms",
                columns: new[] { "mtd_policy", "mtd_form" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_policy_parts",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mtd_policy_parts_mtd_form_part",
                table: "mtd_policy_parts",
                column: "mtd_form_part");

            migrationBuilder.CreateIndex(
                name: "UNIQUE_PART",
                table: "mtd_policy_parts",
                columns: new[] { "mtd_policy", "mtd_form_part" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mtd_policy_scripts_mtd_policy_id",
                table: "mtd_policy_scripts",
                column: "mtd_policy_id");

            migrationBuilder.CreateIndex(
                name: "Unique_id",
                table: "mtd_policy_scripts",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Unique_Policy_Script",
                table: "mtd_policy_scripts",
                columns: new[] { "mtd_filter_script_id", "mtd_policy_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_mtd_store_mtd_form1_idx",
                table: "mtd_store",
                column: "mtd_form");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_store",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TIMECR",
                table: "mtd_store",
                column: "timecr");

            migrationBuilder.CreateIndex(
                name: "Seq_Unique",
                table: "mtd_store",
                columns: new[] { "mtd_form", "sequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_store_approve_stage_idx",
                table: "mtd_store_approval",
                column: "md_approve_stage");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_store_approval",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_APPROVED",
                table: "mtd_store_approval",
                column: "complete");

            migrationBuilder.CreateIndex(
                name: "IX_DATE_RESULT",
                table: "mtd_store_date",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_date_FieldId",
                table: "mtd_store_date",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_date_StoreId",
                table: "mtd_store_date",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_DECIMAL_RESULT",
                table: "mtd_store_decimal",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_decimal_FieldId",
                table: "mtd_store_decimal",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_decimal_StoreId",
                table: "mtd_store_decimal",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_file_FieldId",
                table: "mtd_store_file",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_file_StoreId",
                table: "mtd_store_file",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_INT_RESULT",
                table: "mtd_store_int",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_int_FieldId",
                table: "mtd_store_int",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_int_StoreId",
                table: "mtd_store_int",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_item_FieldId",
                table: "mtd_store_item",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_item_ItemId",
                table: "mtd_store_item",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_item_StoreId",
                table: "mtd_store_item",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TEXT_RESULT",
                table: "mtd_store_item",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_MEMO_RESULT",
                table: "mtd_store_memo",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_memo_FieldId",
                table: "mtd_store_memo",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_memo_StoreId",
                table: "mtd_store_memo",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_store_owner",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER",
                table: "mtd_store_owner",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_text_FieldId",
                table: "mtd_store_text",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_mtd_store_text_StoreId",
                table: "mtd_store_text",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TEXT_RESULT1",
                table: "mtd_store_text",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_sys_style",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_sys_term",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "mtd_sys_type",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mtd_approval_rejection");

            migrationBuilder.DropTable(
                name: "mtd_approval_resolution");

            migrationBuilder.DropTable(
                name: "mtd_config_file");

            migrationBuilder.DropTable(
                name: "mtd_config_param");

            migrationBuilder.DropTable(
                name: "mtd_event_subscribe");

            migrationBuilder.DropTable(
                name: "mtd_filter_column");

            migrationBuilder.DropTable(
                name: "mtd_filter_date");

            migrationBuilder.DropTable(
                name: "mtd_filter_field");

            migrationBuilder.DropTable(
                name: "mtd_filter_owner");

            migrationBuilder.DropTable(
                name: "mtd_filter_script_apply");

            migrationBuilder.DropTable(
                name: "mtd_form_desk");

            migrationBuilder.DropTable(
                name: "mtd_form_header");

            migrationBuilder.DropTable(
                name: "mtd_form_part_header");

            migrationBuilder.DropTable(
                name: "mtd_group");

            migrationBuilder.DropTable(
                name: "mtd_log_approval");

            migrationBuilder.DropTable(
                name: "mtd_log_document");

            migrationBuilder.DropTable(
                name: "mtd_policy_forms");

            migrationBuilder.DropTable(
                name: "mtd_policy_parts");

            migrationBuilder.DropTable(
                name: "mtd_policy_scripts");

            migrationBuilder.DropTable(
                name: "mtd_store_approval");

            migrationBuilder.DropTable(
                name: "mtd_store_date");

            migrationBuilder.DropTable(
                name: "mtd_store_decimal");

            migrationBuilder.DropTable(
                name: "mtd_store_file");

            migrationBuilder.DropTable(
                name: "mtd_store_int");

            migrationBuilder.DropTable(
                name: "mtd_store_item");

            migrationBuilder.DropTable(
                name: "mtd_store_memo");

            migrationBuilder.DropTable(
                name: "mtd_store_owner");

            migrationBuilder.DropTable(
                name: "mtd_store_text");

            migrationBuilder.DropTable(
                name: "mtd_sys_term");

            migrationBuilder.DropTable(
                name: "mtd_filter");

            migrationBuilder.DropTable(
                name: "mtd_filter_script");

            migrationBuilder.DropTable(
                name: "mtd_policy");

            migrationBuilder.DropTable(
                name: "mtd_approval_stage");

            migrationBuilder.DropTable(
                name: "mtd_form_part_field_item");

            migrationBuilder.DropTable(
                name: "mtd_store");

            migrationBuilder.DropTable(
                name: "mtd_approval");

            migrationBuilder.DropTable(
                name: "mtd_form_part_field");

            migrationBuilder.DropTable(
                name: "mtd_form_part");

            migrationBuilder.DropTable(
                name: "mtd_sys_type");

            migrationBuilder.DropTable(
                name: "mtd_form");

            migrationBuilder.DropTable(
                name: "mtd_sys_style");

            migrationBuilder.DropTable(
                name: "mtd_category_form");
        }
    }
}
