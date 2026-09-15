namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Admins.DTOs;
using Huwiyati.Domain.Constants;
using Huwiyati.Domain.Entities.Organizations;

public class AssignAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmployeeNumberGenerator _employeeNumberGenerator;

    public AssignAdminHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmployeeNumberGenerator employeeNumberGenerator)
    {
        _context = context;
        _identityService = identityService;
        _employeeNumberGenerator = employeeNumberGenerator;
    }

    public async Task<ApiResponse<AdminDto>> AssignAdminAsync(
        AssignAdminCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify citizen exists in Civil Registry by National Number
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<AdminDto>.Failure(
                "Citizen with the provided National Number was not found in Civil Registry.",
                statusCode: 404);
        }

        // 2. Get UserId and verify that user account exists and is Active
        var userId = await _identityService.GetUserIdByNationalNumberAsync(command.NationalNumber, cancellationToken);

        if (!userId.HasValue || !await _identityService.IsUserActiveAsync(userId.Value, cancellationToken))
        {
            return ApiResponse<AdminDto>.Failure(
                "Citizen does not have an active user account in the system.",
                statusCode: 400);
        }

        // 3. Verify target branch exists and is active without Include
        var branch = await _context.OrganizationBranches
            .Where(b => b.Id == command.BranchId)
            .Select(b => new
            {
                b.Id,
                b.BranchName,
                b.IsActive,
                OrganizationName = b.Organization.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (branch == null || !branch.IsActive)
        {
            return ApiResponse<AdminDto>.Failure(
                "The specified organization branch was not found or is inactive.",
                statusCode: 404);
        }

        // 4. Check if citizen is already assigned as an employee/admin
        var existingEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.UserId == userId.Value, cancellationToken);

        if (existingEmployee != null)
        {
            return ApiResponse<AdminDto>.Failure(
                "This citizen is already assigned as an employee or admin to a branch.",
                statusCode: 400);
        }

        // 5. Generate employee number automatically using algorithm (e.g. 01-2026-001-0001)
        string employeeNumber = await _employeeNumberGenerator.GenerateEmployeeNumberAsync(command.BranchId, cancellationToken);

        // 6. Create Employee record in database
        var employee = new Employee
        {
            UserId = userId.Value,
            BranchId = command.BranchId,
            EmployeeNumber = employeeNumber,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Employees.AddAsync(employee, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 7. Assign BOTH Admin and Employee roles using IdentityService
        await _identityService.AssignUserRolesAsync(userId.Value, new[] { AppRoles.Admin, AppRoles.Employee }, cancellationToken);

        // 8. Construct response DTO
        var adminDto = new AdminDto
        {
            EmployeeId = employee.Id,
            UserId = userId.Value,
            NationalNumber = person.NationalNumber,
            FullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}",
            EmployeeNumber = employee.EmployeeNumber,
            BranchId = branch.Id,
            BranchName = branch.BranchName,
            OrganizationName = branch.OrganizationName,
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt
        };

        return ApiResponse<AdminDto>.Success(
            adminDto,
            message: "Admin assigned to branch successfully with Admin and Employee roles.");
    }
}
