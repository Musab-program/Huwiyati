namespace Huwiyati.Application.Organizations.Employees.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Employees.DTOs;
using Huwiyati.Domain.Constants;
using Huwiyati.Domain.Entities.Organizations;

public class AssignEmployeeHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmployeeNumberGenerator _employeeNumberGenerator;

    public AssignEmployeeHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmployeeNumberGenerator employeeNumberGenerator)
    {
        _context = context;
        _identityService = identityService;
        _employeeNumberGenerator = employeeNumberGenerator;
    }

    public async Task<ApiResponse<EmployeeDto>> AssignEmployeeAsync(
        Guid currentAdminUserId,
        AssignEmployeeCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Resolve the branch of the current admin from Token UserId
        var adminBranchId = await _context.Employees
            .Where(e => e.UserId == currentAdminUserId && e.IsActive)
            .Select(e => (Guid?)e.BranchId)
            .FirstOrDefaultAsync(cancellationToken);

        if (!adminBranchId.HasValue)
        {
            return ApiResponse<EmployeeDto>.Failure(
                "Current logged-in user is not assigned as an active admin to any branch.",
                statusCode: 403);
        }

        // 2. Verify citizen exists in Civil Registry by National Number
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<EmployeeDto>.Failure(
                "Citizen with the provided National Number was not found in Civil Registry.",
                statusCode: 404);
        }

        // 3. Get target UserId and verify user account exists and is Active
        var userId = await _identityService.GetUserIdByNationalNumberAsync(command.NationalNumber, cancellationToken);

        if (!userId.HasValue || !await _identityService.IsUserActiveAsync(userId.Value, cancellationToken))
        {
            return ApiResponse<EmployeeDto>.Failure(
                "Citizen does not have an active user account in the system.",
                statusCode: 400);
        }

        // 4. Check if citizen is already assigned as an employee or admin in any branch
        var existingEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.UserId == userId.Value, cancellationToken);

        if (existingEmployee != null)
        {
            return ApiResponse<EmployeeDto>.Failure(
                "This citizen is already assigned as an employee or admin to a branch.",
                statusCode: 400);
        }

        // 5. Generate employee number automatically using algorithm (e.g. 01-2026-001-0001)
        string employeeNumber = await _employeeNumberGenerator.GenerateEmployeeNumberAsync(adminBranchId.Value, cancellationToken);

        // 6. Create Employee record in database
        var employee = new Employee
        {
            UserId = userId.Value,
            BranchId = adminBranchId.Value,
            EmployeeNumber = employeeNumber,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Employees.AddAsync(employee, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 7. Assign Employee role using IdentityService
        await _identityService.AssignUserRolesAsync(userId.Value, new[] { AppRoles.Employee }, cancellationToken);

        // 8. Fetch branch details for response DTO
        var branchInfo = await _context.OrganizationBranches
            .Where(b => b.Id == adminBranchId.Value)
            .Select(b => new { BranchName = b.BranchName, OrganizationName = b.Organization.Name })
            .FirstOrDefaultAsync(cancellationToken);

        // 9. Construct response DTO
        var employeeDto = new EmployeeDto
        {
            EmployeeId = employee.Id,
            UserId = userId.Value,
            NationalNumber = person.NationalNumber,
            FullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}",
            EmployeeNumber = employee.EmployeeNumber,
            BranchId = adminBranchId.Value,
            BranchName = branchInfo?.BranchName ?? string.Empty,
            OrganizationName = branchInfo?.OrganizationName ?? string.Empty,
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt
        };

        return ApiResponse<EmployeeDto>.Success(
            employeeDto,
            message: "Employee assigned to branch successfully with Employee role.");
    }
}
