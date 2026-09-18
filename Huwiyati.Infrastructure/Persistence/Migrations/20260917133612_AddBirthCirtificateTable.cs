using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Huwiyati.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBirthCirtificateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BirthCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FatherPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MotherPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HospitalBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirthCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BirthCertificates_OrganizationBranches_HospitalBranchId",
                        column: x => x.HospitalBranchId,
                        principalTable: "OrganizationBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BirthCertificates_Persons_ChildPersonId",
                        column: x => x.ChildPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BirthCertificates_Persons_FatherPersonId",
                        column: x => x.FatherPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BirthCertificates_Persons_MotherPersonId",
                        column: x => x.MotherPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BirthCertificates_CertificateNumber",
                table: "BirthCertificates",
                column: "CertificateNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BirthCertificates_ChildPersonId",
                table: "BirthCertificates",
                column: "ChildPersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BirthCertificates_FatherPersonId",
                table: "BirthCertificates",
                column: "FatherPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_BirthCertificates_HospitalBranchId",
                table: "BirthCertificates",
                column: "HospitalBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BirthCertificates_MotherPersonId",
                table: "BirthCertificates",
                column: "MotherPersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BirthCertificates");
        }
    }
}
