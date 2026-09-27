using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class UpdateKamiReviewForBackwardsActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_kami_review_users_reviewed_by",
                table: "kami_review");

            migrationBuilder.RenameColumn(
                name: "reviewed_by",
                table: "kami_review",
                newName: "resolved_by");

            migrationBuilder.RenameColumn(
                name: "reviewed_at",
                table: "kami_review",
                newName: "resolved_at");

            migrationBuilder.RenameIndex(
                name: "IX_kami_review_reviewed_by",
                table: "kami_review",
                newName: "IX_kami_review_resolved_by");

            migrationBuilder.AddColumn<DateTime>(
                name: "returned_to_draft_at",
                table: "kami_review",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "returned_to_draft_by",
                table: "kami_review",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_kami_review_returned_to_draft_by",
                table: "kami_review",
                column: "returned_to_draft_by");

            migrationBuilder.AddForeignKey(
                name: "FK_kami_review_users_resolved_by",
                table: "kami_review",
                column: "resolved_by",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_kami_review_users_returned_to_draft_by",
                table: "kami_review",
                column: "returned_to_draft_by",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_kami_review_users_resolved_by",
                table: "kami_review");

            migrationBuilder.DropForeignKey(
                name: "FK_kami_review_users_returned_to_draft_by",
                table: "kami_review");

            migrationBuilder.DropIndex(
                name: "IX_kami_review_returned_to_draft_by",
                table: "kami_review");

            migrationBuilder.DropColumn(
                name: "returned_to_draft_at",
                table: "kami_review");

            migrationBuilder.DropColumn(
                name: "returned_to_draft_by",
                table: "kami_review");

            migrationBuilder.RenameColumn(
                name: "resolved_by",
                table: "kami_review",
                newName: "reviewed_by");

            migrationBuilder.RenameColumn(
                name: "resolved_at",
                table: "kami_review",
                newName: "reviewed_at");

            migrationBuilder.RenameIndex(
                name: "IX_kami_review_resolved_by",
                table: "kami_review",
                newName: "IX_kami_review_reviewed_by");

            migrationBuilder.AddForeignKey(
                name: "FK_kami_review_users_reviewed_by",
                table: "kami_review",
                column: "reviewed_by",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}