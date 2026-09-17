using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Huwiyati.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFamlayFeaturesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HeadOfFamilyPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuingBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    QrCodePayload = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Families_OrganizationBranches_IssuingBranchId",
                        column: x => x.IssuingBranchId,
                        principalTable: "OrganizationBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Families_Persons_HeadOfFamilyPersonId",
                        column: x => x.HeadOfFamilyPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarriageContracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HusbandPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WifePersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarriageDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DocumentPhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    ApprovedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarriageContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarriageContracts_Persons_HusbandPersonId",
                        column: x => x.HusbandPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarriageContracts_Persons_WifePersonId",
                        column: x => x.WifePersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FamilyMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarriageContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelationshipType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeftAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_MarriageContracts_MarriageContractId",
                        column: x => x.MarriageContractId,
                        principalTable: "MarriageContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Families_FamilyNumber",
                table: "Families",
                column: "FamilyNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Families_HeadOfFamilyPersonId",
                table: "Families",
                column: "HeadOfFamilyPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_IssuingBranchId",
                table: "Families",
                column: "IssuingBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_FamilyId",
                table: "FamilyMembers",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_MarriageContractId",
                table: "FamilyMembers",
                column: "MarriageContractId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_PersonId",
                table: "FamilyMembers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_MarriageContracts_ContractNumber",
                table: "MarriageContracts",
                column: "ContractNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarriageContracts_HusbandPersonId",
                table: "MarriageContracts",
                column: "HusbandPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_MarriageContracts_WifePersonId",
                table: "MarriageContracts",
                column: "WifePersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyMembers");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "MarriageContracts");
        }
    }
}
