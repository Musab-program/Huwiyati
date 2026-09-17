using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Huwiyati.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditNationalNumberCardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NationalIdCards_OrganizationBranches_OrganizationBranchId",
                table: "NationalIdCards");

            migrationBuilder.DropForeignKey(
                name: "FK_NationalIdCards_Persons_PersonId1",
                table: "NationalIdCards");

            migrationBuilder.DropIndex(
                name: "IX_NationalIdCards_OrganizationBranchId",
                table: "NationalIdCards");

            migrationBuilder.DropIndex(
                name: "IX_NationalIdCards_PersonId1",
                table: "NationalIdCards");

            migrationBuilder.DropColumn(
                name: "OrganizationBranchId",
                table: "NationalIdCards");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "NationalIdCards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationBranchId",
                table: "NationalIdCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PersonId1",
                table: "NationalIdCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NationalIdCards_OrganizationBranchId",
                table: "NationalIdCards",
                column: "OrganizationBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_NationalIdCards_PersonId1",
                table: "NationalIdCards",
                column: "PersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_NationalIdCards_OrganizationBranches_OrganizationBranchId",
                table: "NationalIdCards",
                column: "OrganizationBranchId",
                principalTable: "OrganizationBranches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NationalIdCards_Persons_PersonId1",
                table: "NationalIdCards",
                column: "PersonId1",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
