using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToGift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Gifts_GiftId",
                table: "Winners");

            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Users_UserId",
                table: "Winners");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Gifts",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Gifts_GiftId",
                table: "Winners",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Users_UserId",
                table: "Winners",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Gifts_GiftId",
                table: "Winners");

            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Users_UserId",
                table: "Winners");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Gifts");

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Gifts_GiftId",
                table: "Winners",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Users_UserId",
                table: "Winners",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
