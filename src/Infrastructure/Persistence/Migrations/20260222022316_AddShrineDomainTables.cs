using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddShrineDomainTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "folklore",
                columns: table => new
                {
                    folklore_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    shrine_id = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    information = table.Column<string>(type: "text", nullable: true),
                    img_id = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_folklore", x => x.folklore_id);
                    table.ForeignKey(
                        name: "FK_folklore_images_img_id",
                        column: x => x.img_id,
                        principalTable: "images",
                        principalColumn: "img_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_folklore_shrines_shrine_id",
                        column: x => x.shrine_id,
                        principalTable: "shrines",
                        principalColumn: "shrine_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "history",
                columns: table => new
                {
                    history_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    shrine_id = table.Column<int>(type: "integer", nullable: false),
                    event_date = table.Column<DateOnly>(type: "date", nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    information = table.Column<string>(type: "text", nullable: true),
                    img_id = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_history", x => x.history_id);
                    table.ForeignKey(
                        name: "FK_history_images_img_id",
                        column: x => x.img_id,
                        principalTable: "images",
                        principalColumn: "img_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_history_shrines_shrine_id",
                        column: x => x.shrine_id,
                        principalTable: "shrines",
                        principalColumn: "shrine_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kami",
                columns: table => new
                {
                    kami_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_en = table.Column<string>(type: "text", nullable: true),
                    name_jp = table.Column<string>(type: "text", nullable: true),
                    img_id = table.Column<int>(type: "integer", nullable: true),
                    desc = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kami", x => x.kami_id);
                    table.ForeignKey(
                        name: "FK_kami_images_img_id",
                        column: x => x.img_id,
                        principalTable: "images",
                        principalColumn: "img_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "shrine_gallery",
                columns: table => new
                {
                    shrine_id = table.Column<int>(type: "integer", nullable: false),
                    img_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shrine_gallery", x => new { x.shrine_id, x.img_id });
                    table.ForeignKey(
                        name: "FK_shrine_gallery_images_img_id",
                        column: x => x.img_id,
                        principalTable: "images",
                        principalColumn: "img_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shrine_gallery_shrines_shrine_id",
                        column: x => x.shrine_id,
                        principalTable: "shrines",
                        principalColumn: "shrine_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_collection",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    shrine_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_collection", x => new { x.user_id, x.shrine_id });
                    table.ForeignKey(
                        name: "FK_user_collection_shrines_shrine_id",
                        column: x => x.shrine_id,
                        principalTable: "shrines",
                        principalColumn: "shrine_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_collection_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "folklore_citations",
                columns: table => new
                {
                    folklore_id = table.Column<int>(type: "integer", nullable: false),
                    cite_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_folklore_citations", x => new { x.folklore_id, x.cite_id });
                    table.ForeignKey(
                        name: "FK_folklore_citations_citations_cite_id",
                        column: x => x.cite_id,
                        principalTable: "citations",
                        principalColumn: "cite_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_folklore_citations_folklore_folklore_id",
                        column: x => x.folklore_id,
                        principalTable: "folklore",
                        principalColumn: "folklore_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "history_citations",
                columns: table => new
                {
                    history_id = table.Column<int>(type: "integer", nullable: false),
                    cite_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_history_citations", x => new { x.history_id, x.cite_id });
                    table.ForeignKey(
                        name: "FK_history_citations_citations_cite_id",
                        column: x => x.cite_id,
                        principalTable: "citations",
                        principalColumn: "cite_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_history_citations_history_history_id",
                        column: x => x.history_id,
                        principalTable: "history",
                        principalColumn: "history_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kami_citations",
                columns: table => new
                {
                    kami_id = table.Column<int>(type: "integer", nullable: false),
                    cite_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kami_citations", x => new { x.kami_id, x.cite_id });
                    table.ForeignKey(
                        name: "FK_kami_citations_citations_cite_id",
                        column: x => x.cite_id,
                        principalTable: "citations",
                        principalColumn: "cite_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_kami_citations_kami_kami_id",
                        column: x => x.kami_id,
                        principalTable: "kami",
                        principalColumn: "kami_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shrine_kami",
                columns: table => new
                {
                    shrine_id = table.Column<int>(type: "integer", nullable: false),
                    kami_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shrine_kami", x => new { x.shrine_id, x.kami_id });
                    table.ForeignKey(
                        name: "FK_shrine_kami_kami_kami_id",
                        column: x => x.kami_id,
                        principalTable: "kami",
                        principalColumn: "kami_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shrine_kami_shrines_shrine_id",
                        column: x => x.shrine_id,
                        principalTable: "shrines",
                        principalColumn: "shrine_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_folklore_img_id",
                table: "folklore",
                column: "img_id");

            migrationBuilder.CreateIndex(
                name: "IX_folklore_shrine_id",
                table: "folklore",
                column: "shrine_id");

            migrationBuilder.CreateIndex(
                name: "IX_folklore_citations_cite_id",
                table: "folklore_citations",
                column: "cite_id");

            migrationBuilder.CreateIndex(
                name: "IX_history_img_id",
                table: "history",
                column: "img_id");

            migrationBuilder.CreateIndex(
                name: "IX_history_shrine_id",
                table: "history",
                column: "shrine_id");

            migrationBuilder.CreateIndex(
                name: "IX_history_citations_cite_id",
                table: "history_citations",
                column: "cite_id");

            migrationBuilder.CreateIndex(
                name: "IX_kami_img_id",
                table: "kami",
                column: "img_id");

            migrationBuilder.CreateIndex(
                name: "IX_kami_citations_cite_id",
                table: "kami_citations",
                column: "cite_id");

            migrationBuilder.CreateIndex(
                name: "IX_shrine_gallery_img_id",
                table: "shrine_gallery",
                column: "img_id");

            migrationBuilder.CreateIndex(
                name: "IX_shrine_kami_kami_id",
                table: "shrine_kami",
                column: "kami_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_collection_shrine_id",
                table: "user_collection",
                column: "shrine_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "folklore_citations");

            migrationBuilder.DropTable(
                name: "history_citations");

            migrationBuilder.DropTable(
                name: "kami_citations");

            migrationBuilder.DropTable(
                name: "shrine_gallery");

            migrationBuilder.DropTable(
                name: "shrine_kami");

            migrationBuilder.DropTable(
                name: "user_collection");

            migrationBuilder.DropTable(
                name: "folklore");

            migrationBuilder.DropTable(
                name: "history");

            migrationBuilder.DropTable(
                name: "kami");
        }
    }
}
