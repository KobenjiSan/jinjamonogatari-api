using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_folklore_citations_citations_cite_id",
                table: "folklore_citations");

            migrationBuilder.DropForeignKey(
                name: "FK_history_citations_citations_cite_id",
                table: "history_citations");

            migrationBuilder.DropForeignKey(
                name: "FK_shrine_gallery_images_img_id",
                table: "shrine_gallery");

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "shrines",
                type: "text",
                maxLength: 10000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_folklore_citations_citations_cite_id",
                table: "folklore_citations",
                column: "cite_id",
                principalTable: "citations",
                principalColumn: "cite_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_history_citations_citations_cite_id",
                table: "history_citations",
                column: "cite_id",
                principalTable: "citations",
                principalColumn: "cite_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_shrine_gallery_images_img_id",
                table: "shrine_gallery",
                column: "img_id",
                principalTable: "images",
                principalColumn: "img_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_folklore_citations_citations_cite_id",
                table: "folklore_citations");

            migrationBuilder.DropForeignKey(
                name: "FK_history_citations_citations_cite_id",
                table: "history_citations");

            migrationBuilder.DropForeignKey(
                name: "FK_shrine_gallery_images_img_id",
                table: "shrine_gallery");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "shrines");

            migrationBuilder.AddForeignKey(
                name: "FK_folklore_citations_citations_cite_id",
                table: "folklore_citations",
                column: "cite_id",
                principalTable: "citations",
                principalColumn: "cite_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_history_citations_citations_cite_id",
                table: "history_citations",
                column: "cite_id",
                principalTable: "citations",
                principalColumn: "cite_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shrine_gallery_images_img_id",
                table: "shrine_gallery",
                column: "img_id",
                principalTable: "images",
                principalColumn: "img_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
