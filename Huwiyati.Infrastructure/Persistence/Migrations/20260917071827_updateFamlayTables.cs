using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Huwiyati.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateFamlayTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Families_FamilyNumber",
                table: "Families");

            migrationBuilder.CreateIndex(
                name: "IX_Families_FamilyNumber",
                table: "Families",
                column: "FamilyNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Families_FamilyNumber",
                table: "Families");

            migrationBuilder.CreateIndex(
                name: "IX_Families_FamilyNumber",
                table: "Families",
                column: "FamilyNumber",
                unique: true);
        }
    }
}
