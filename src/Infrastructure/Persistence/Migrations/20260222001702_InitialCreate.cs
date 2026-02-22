using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

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
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citations", x => x.cite_id);
                });

            migrationBuilder.CreateTable(
                name: "etiquette_topics",
                columns: table => new
                {
                    topic_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slug = table.Column<string>(type: "text", nullable: false),
                    title_long = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    summary = table.Column<string>(type: "text", nullable: true),
                    show_in_glance = table.Column<bool>(type: "boolean", nullable: false),
                    title_short = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    icon_key = table.Column<string>(type: "text", nullable: true),
                    icon_set = table.Column<string>(type: "text", nullable: true),
                    glance_order = table.Column<int>(type: "integer", nullable: true),
                    show_as_highlight = table.Column<bool>(type: "boolean", nullable: false),
                    guide_order = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etiquette_topics", x => x.topic_id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title_en = table.Column<string>(type: "text", nullable: false),
                    title_jp = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    pass_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    role_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                name: "etiquette_citations",
                columns: table => new
                {
                    topic_id = table.Column<int>(type: "integer", nullable: false),
                    cite_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    text = table.Column<string>(type: "text", nullable: true),
                    step_order = table.Column<int>(type: "integer", nullable: true),
                    img_id = table.Column<int>(type: "integer", nullable: true),
                    topic_id = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "shrines",
                columns: table => new
                {
                    shrine_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    inputted_id = table.Column<string>(type: "text", nullable: true),
                    lat = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    lon = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    location = table.Column<Point>(type: "geography(point,4326)", nullable: true),
                    slug = table.Column<string>(type: "text", nullable: true),
                    name_en = table.Column<string>(type: "text", nullable: true),
                    name_jp = table.Column<string>(type: "text", nullable: true),
                    shrine_desc = table.Column<string>(type: "text", nullable: true),
                    address_raw = table.Column<string>(type: "text", nullable: true),
                    prefecture = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    ward = table.Column<string>(type: "text", nullable: true),
                    locality = table.Column<string>(type: "text", nullable: true),
                    postal_code = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    website = table.Column<string>(type: "text", nullable: true),
                    img_id = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shrines", x => x.shrine_id);
                    table.ForeignKey(
                        name: "FK_shrines_images_img_id",
                        column: x => x.img_id,
                        principalTable: "images",
                        principalColumn: "img_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "shrine_tags",
                columns: table => new
                {
                    shrine_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shrine_tags", x => new { x.shrine_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_shrine_tags_shrines_shrine_id",
                        column: x => x.shrine_id,
                        principalTable: "shrines",
                        principalColumn: "shrine_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shrine_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_etiquette_topics_slug",
                table: "etiquette_topics",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_images_cite_id",
                table: "images",
                column: "cite_id");

            migrationBuilder.CreateIndex(
                name: "IX_shrine_tags_tag_id",
                table: "shrine_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_shrines_img_id",
                table: "shrines",
                column: "img_id");

            migrationBuilder.CreateIndex(
                name: "IX_shrines_slug",
                table: "shrines",
                column: "slug",
                unique: true,
                filter: "slug IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tags_title_en",
                table: "tags",
                column: "title_en",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "etiquette_citations");

            migrationBuilder.DropTable(
                name: "etiquette_steps");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "shrine_tags");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "etiquette_topics");

            migrationBuilder.DropTable(
                name: "shrines");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "citations");
        }
    }
}
