using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddShrineReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shrine_review",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    shrine_id = table.Column<int>(type: "integer", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    submitted_by = table.Column<int>(type: "integer", nullable: false),
                    reviewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reviewed_by = table.Column<int>(type: "integer", nullable: true),
                    reviewer_comment = table.Column<string>(type: "text", nullable: true),
                    decision = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shrine_review", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_shrine_review_shrines_shrine_id",
                        column: x => x.shrine_id,
                        principalTable: "shrines",
                        principalColumn: "shrine_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shrine_review_users_reviewed_by",
                        column: x => x.reviewed_by,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_shrine_review_users_submitted_by",
                        column: x => x.submitted_by,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shrine_review_reviewed_by",
                table: "shrine_review",
                column: "reviewed_by");

            migrationBuilder.CreateIndex(
                name: "IX_shrine_review_shrine_id",
                table: "shrine_review",
                column: "shrine_id");

            migrationBuilder.CreateIndex(
                name: "IX_shrine_review_submitted_by",
                table: "shrine_review",
                column: "submitted_by");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shrine_review");
        }
    }
}
