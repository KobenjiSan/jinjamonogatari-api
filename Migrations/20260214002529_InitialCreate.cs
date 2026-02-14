using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "citations",
                columns: table => new
                {
                    cite_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    author = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    year = table.Column<int>(type: "integer", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citations", x => x.cite_id);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    img_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    img_source = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    desc = table.Column<string>(type: "text", nullable: true),
                    cite_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.img_id);
                    table.ForeignKey(
                        name: "FK_images_citations_cite_id",
                        column: x => x.cite_id,
                        principalTable: "citations",
                        principalColumn: "cite_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "etiquette_topics",
                columns: table => new
                {
                    topic_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slug = table.Column<string>(type: "text", nullable: false),
                    title_long = table.Column<string>(type: "text", nullable: true),
                    title_short = table.Column<string>(type: "text", nullable: true),
                    summary = table.Column<string>(type: "text", nullable: true),
                    icon_key = table.Column<string>(type: "text", nullable: true),
                    icon_set = table.Column<string>(type: "text", nullable: true),
                    img_id = table.Column<int>(type: "integer", nullable: true),
                    show_in_glance = table.Column<bool>(type: "boolean", nullable: false),
                    show_as_highlight = table.Column<bool>(type: "boolean", nullable: false),
                    glance_order = table.Column<int>(type: "integer", nullable: true),
                    guide_order = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etiquette_topics", x => x.topic_id);
                    table.ForeignKey(
                        name: "FK_etiquette_topics_images_img_id",
                        column: x => x.img_id,
                        principalTable: "images",
                        principalColumn: "img_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "etiquette_citations",
                columns: table => new
                {
                    topic_id = table.Column<int>(type: "integer", nullable: false),
                    cite_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etiquette_citations", x => new { x.topic_id, x.cite_id });
                    table.ForeignKey(
                        name: "FK_etiquette_citations_citations_cite_id",
                        column: x => x.cite_id,
                        principalTable: "citations",
                        principalColumn: "cite_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_etiquette_citations_etiquette_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "etiquette_topics",
                        principalColumn: "topic_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "etiquette_steps",
                columns: table => new
                {
                    step_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    topic_id = table.Column<int>(type: "integer", nullable: false),
                    step_order = table.Column<int>(type: "integer", nullable: true),
                    text = table.Column<string>(type: "text", nullable: true),
                    img_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etiquette_steps", x => x.step_id);
                    table.ForeignKey(
                        name: "FK_etiquette_steps_etiquette_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "etiquette_topics",
                        principalColumn: "topic_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_etiquette_steps_images_img_id",
                        column: x => x.img_id,
                        principalTable: "images",
                        principalColumn: "img_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_etiquette_citations_cite_id",
                table: "etiquette_citations",
                column: "cite_id");

            migrationBuilder.CreateIndex(
                name: "IX_etiquette_steps_img_id",
                table: "etiquette_steps",
                column: "img_id");

            migrationBuilder.CreateIndex(
                name: "IX_etiquette_steps_topic_id_step_order",
                table: "etiquette_steps",
                columns: new[] { "topic_id", "step_order" });

            migrationBuilder.CreateIndex(
                name: "IX_etiquette_topics_img_id",
                table: "etiquette_topics",
                column: "img_id");

            migrationBuilder.CreateIndex(
                name: "IX_etiquette_topics_slug",
                table: "etiquette_topics",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_images_cite_id",
                table: "images",
                column: "cite_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "etiquette_citations");

            migrationBuilder.DropTable(
                name: "etiquette_steps");

            migrationBuilder.DropTable(
                name: "etiquette_topics");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "citations");
        }
    }
}
