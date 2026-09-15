using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Huwiyati.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixEmployeeRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Employees_EmployeeId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_OrganizationBranches_OrganizationBranchId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_OrganizationBranchId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_EmployeeId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationBranchId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "ApplicationUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationBranchId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "ApplicationUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_OrganizationBranchId",
                table: "Employees",
                column: "OrganizationBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_EmployeeId",
                table: "ApplicationUsers",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Employees_EmployeeId",
                table: "ApplicationUsers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_OrganizationBranches_OrganizationBranchId",
                table: "Employees",
                column: "OrganizationBranchId",
                principalTable: "OrganizationBranches",
                principalColumn: "Id");
        }
    }
}
