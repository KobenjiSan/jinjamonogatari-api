using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenUserFkAndRotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RefreshTokens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "RefreshTokens",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
