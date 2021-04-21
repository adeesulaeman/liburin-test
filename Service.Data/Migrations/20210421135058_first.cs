using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Service.Data.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "discount_input_type",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    flag = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discount_input_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discount_type",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    flag = table.Column<string>(nullable: true),
                    operation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discount_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "voucher_type",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_voucher_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "voucher",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: false),
                    value = table.Column<int>(nullable: false),
                    period_from = table.Column<DateTime>(nullable: false),
                    period_until = table.Column<DateTime>(nullable: false),
                    voucher_code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    qty_voucher = table.Column<int>(nullable: false),
                    max_qty_per_user = table.Column<int>(nullable: false),
                    max_qty_per_user_per_day = table.Column<int>(nullable: false),
                    min_order_amount = table.Column<int>(nullable: false),
                    max_discount = table.Column<int>(nullable: false),
                    limit_per_day = table.Column<int>(nullable: false),
                    is_public = table.Column<int>(nullable: false),
                    discount_type_id = table.Column<long>(nullable: false),
                    voucher_type_id = table.Column<long>(nullable: false),
                    discount_input_type_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_voucher", x => x.id);
                    table.ForeignKey(
                        name: "fk_voucher_discount_input_type_discount_input_type_id",
                        column: x => x.discount_input_type_id,
                        principalTable: "discount_input_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_voucher_discount_type_discount_type_id",
                        column: x => x.discount_type_id,
                        principalTable: "discount_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_voucher_voucher_type_voucher_type_id",
                        column: x => x.voucher_type_id,
                        principalTable: "voucher_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promo",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    caption = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    image_url = table.Column<string>(nullable: true),
                    image_storage_name = table.Column<string>(nullable: true),
                    period_from = table.Column<DateTime>(nullable: false),
                    period_until = table.Column<DateTime>(nullable: false),
                    voucher_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promo", x => x.id);
                    table.ForeignKey(
                        name: "fk_promo_voucher_voucher_id",
                        column: x => x.voucher_id,
                        principalTable: "voucher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_promo_voucher_id",
                table: "promo",
                column: "voucher_id");

            migrationBuilder.CreateIndex(
                name: "ix_voucher_discount_input_type_id",
                table: "voucher",
                column: "discount_input_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_voucher_discount_type_id",
                table: "voucher",
                column: "discount_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_voucher_voucher_type_id",
                table: "voucher",
                column: "voucher_type_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "promo");

            migrationBuilder.DropTable(
                name: "voucher");

            migrationBuilder.DropTable(
                name: "discount_input_type");

            migrationBuilder.DropTable(
                name: "discount_type");

            migrationBuilder.DropTable(
                name: "voucher_type");
        }
    }
}
