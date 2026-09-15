namespace Huwiyati.Application.Organizations.Employees.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Employees.DTOs;

public class UpdateEmployeeHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmployeeNumberGenerator _employeeNumberGenerator;

    public UpdateEmployeeHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmployeeNumberGenerator employeeNumberGenerator)
    {
        _context = context;
        _identityService = identityService;
        _employeeNumberGenerator = employeeNumberGenerator;
    }

    public async Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(
        Guid currentAdminUserId,
        UpdateEmployeeCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify current admin authority and resolve admin branch
        var adminBranchId = await _context.Employees
            .Where(e => e.UserId == currentAdminUserId && e.IsActive)
            .Select(e => (Guid?)e.BranchId)
            .FirstOrDefaultAsync(cancellationToken);

        if (!adminBranchId.HasValue)
        {
            return ApiResponse<EmployeeDto>.Failure("Current logged-in user is not assigned as an active admin to any branch.", statusCode: 403);
        }

        // 2. Ensure current admin is transferring/updating employee ONLY to their own branch
        if (command.NewBranchId != adminBranchId.Value)
        {
            return ApiResponse<EmployeeDto>.Failure("Admin can only transfer or assign employees to their own branch.", statusCode: 403);
        }

        // 3. Fetch employee record from database
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == command.EmployeeId, cancellationToken);

        if (employee == null)
        {
            return ApiResponse<EmployeeDto>.Failure("Employee record was not found.", statusCode: 404);
        }

        // 4. Verify target new branch exists and is active
        var targetBranch = await _context.OrganizationBranches
            .Where(b => b.Id == command.NewBranchId)
            .Select(b => new
            {
                b.Id,
                b.BranchName,
                b.IsActive,
                OrganizationName = b.Organization.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (targetBranch == null || !targetBranch.IsActive)
        {
            return ApiResponse<EmployeeDto>.Failure("Target organization branch was not found or is inactive.", statusCode: 404);
        }

        // 5. Check if employee is already assigned to this branch
        if (employee.BranchId == command.NewBranchId)
        {
            return ApiResponse<EmployeeDto>.Failure("Employee is already assigned to this branch.", statusCode: 400);
        }

        // 6. Generate new EmployeeNumber for the target branch
        string newEmployeeNumber = await _employeeNumberGenerator.GenerateEmployeeNumberAsync(command.NewBranchId, cancellationToken);

        // 7. Update employee branch and employee number
        employee.BranchId = command.NewBranchId;
        employee.EmployeeNumber = newEmployeeNumber;

        _context.Employees.Update(employee);
        await _context.SaveChangesAsync(cancellationToken);

        // 8. Resolve Person details via IdentityService
        var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(employee.UserId, cancellationToken);
        
        string nationalNumber = string.Empty;
        string fullName = string.Empty;

        if (contactInfo.HasValue && contactInfo.Value.PersonId != Guid.Empty)
        {
            var person = await _context.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == contactInfo.Value.PersonId, cancellationToken);

            if (person != null)
            {
                nationalNumber = person.NationalNumber;
                fullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}";
            }
        }

        // 9. Construct response DTO
        var dto = new EmployeeDto
        {
            EmployeeId = employee.Id,
            UserId = employee.UserId,
            NationalNumber = nationalNumber,
            FullName = fullName,
            Email = contactInfo?.Email ?? string.Empty,
            PhoneNumber = contactInfo?.PhoneNumber ?? string.Empty,
            EmployeeNumber = employee.EmployeeNumber,
            BranchId = targetBranch.Id,
            BranchName = targetBranch.BranchName,
            OrganizationName = targetBranch.OrganizationName,
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt
        };

        return ApiResponse<EmployeeDto>.Success(dto, message: "Employee branch updated successfully.");
    }
}
