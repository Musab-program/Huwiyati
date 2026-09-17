using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Huwiyati.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNationalNumberCardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BloodGroup",
                table: "Persons",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "NationalIdCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuingBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    QrCodePayload = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    OrganizationBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PersonId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NationalIdCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NationalIdCards_OrganizationBranches_IssuingBranchId",
                        column: x => x.IssuingBranchId,
                        principalTable: "OrganizationBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NationalIdCards_OrganizationBranches_OrganizationBranchId",
                        column: x => x.OrganizationBranchId,
                        principalTable: "OrganizationBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NationalIdCards_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NationalIdCards_Persons_PersonId1",
                        column: x => x.PersonId1,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NationalIdCards_IssuingBranchId",
                table: "NationalIdCards",
                column: "IssuingBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_NationalIdCards_OrganizationBranchId",
                table: "NationalIdCards",
                column: "OrganizationBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_NationalIdCards_PersonId",
                table: "NationalIdCards",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_NationalIdCards_PersonId1",
                table: "NationalIdCards",
                column: "PersonId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NationalIdCards");

            migrationBuilder.AlterColumn<string>(
                name: "BloodGroup",
                table: "Persons",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
