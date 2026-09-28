using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityAudits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "entity_audit_id",
                table: "kami",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "entity_audit",
                columns: table => new
                {
                    entity_audit_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    error_count = table.Column<int>(type: "integer", nullable: false),
                    warning_count = table.Column<int>(type: "integer", nullable: false),
                    can_submit = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_audit", x => x.entity_audit_id);
                });

            migrationBuilder.CreateTable(
                name: "entity_audit_issue",
                columns: table => new
                {
                    entity_audit_issue_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_audit_id = table.Column<int>(type: "integer", nullable: false),
                    severity = table.Column<string>(type: "text", nullable: false),
                    field = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    related_item_type = table.Column<string>(type: "text", nullable: true),
                    related_item_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_audit_issue", x => x.entity_audit_issue_id);
                    table.ForeignKey(
                        name: "FK_entity_audit_issue_entity_audit_entity_audit_id",
                        column: x => x.entity_audit_id,
                        principalTable: "entity_audit",
                        principalColumn: "entity_audit_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kami_entity_audit_id",
                table: "kami",
                column: "entity_audit_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_entity_audit_issue_entity_audit_id",
                table: "entity_audit_issue",
                column: "entity_audit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_kami_entity_audit_entity_audit_id",
                table: "kami",
                column: "entity_audit_id",
                principalTable: "entity_audit",
                principalColumn: "entity_audit_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_kami_entity_audit_entity_audit_id",
                table: "kami");

            migrationBuilder.DropTable(
                name: "entity_audit_issue");

            migrationBuilder.DropTable(
                name: "entity_audit");

            migrationBuilder.DropIndex(
                name: "IX_kami_entity_audit_id",
                table: "kami");

            migrationBuilder.DropColumn(
                name: "entity_audit_id",
                table: "kami");
        }
    }
}
