using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConvertKamiStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE kami
                SET status = CASE
                    WHEN status IS NULL OR status = '' THEN 'Draft'
                    WHEN LOWER(status) = 'draft' THEN 'Draft'
                    WHEN LOWER(status) = 'review' THEN 'Review'
                    WHEN LOWER(status) = 'published' THEN 'Published'
                    ELSE status
                END;
            """);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "kami",
                type: "text",
                nullable: false,
                defaultValue: "Draft",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "kami",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
