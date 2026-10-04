using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Huwiyati.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQrCodePayloadToCertificates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrCodePayload",
                table: "DeathCertificates",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrCodePayload",
                table: "BirthCertificates",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_ApplicationUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_ApplicationUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "QrCodePayload",
                table: "DeathCertificates");

            migrationBuilder.DropColumn(
                name: "QrCodePayload",
                table: "BirthCertificates");
        }
    }
}
