namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Admins.DTOs;
using Huwiyati.Domain.Constants;
using Huwiyati.Domain.Entities.Organizations;

using Huwiyati.Domain.Events.Organizations;

/// <summary>
/// Handles assigning a citizen as an administrator to an organization branch, creating or updating employee records and user roles.
/// </summary>
public class AssignAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmployeeNumberGenerator _employeeNumberGenerator;
    private readonly IDomainEventHandler<AdminAssignedEvent> _adminAssignedEventHandler;
    private readonly IBranchAdminValidator _branchAdminValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignAdminHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="employeeNumberGenerator">The generator for standardized employee numbers.</param>
    /// <param name="adminAssignedEventHandler">The event handler for admin assignment domain events.</param>
    /// <param name="branchAdminValidator">The validator ensuring single admin per branch.</param>
    public AssignAdminHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmployeeNumberGenerator employeeNumberGenerator,
        IDomainEventHandler<AdminAssignedEvent> adminAssignedEventHandler,
        IBranchAdminValidator branchAdminValidator)
    {
        _context = context;
        _identityService = identityService;
        _employeeNumberGenerator = employeeNumberGenerator;
        _adminAssignedEventHandler = adminAssignedEventHandler;
        _branchAdminValidator = branchAdminValidator;
    }

    /// <summary>
    /// Assigns administrative rights to a citizen for a branch asynchronously.
    /// </summary>
    /// <param name="command">The assign admin command payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the resulting <see cref="AdminDto"/>.</returns>
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

        if (person.PersonStatus == Huwiyati.Domain.Enums.PersonStatus.Deceased)
        {
            return ApiResponse<AdminDto>.Failure(
                "Cannot assign a deceased citizen as an administrator.",
                statusCode: 400);
        }

        // 2. Get UserId and verify that user account exists and is Active
        var userId = await _identityService.GetUserIdByNationalNumberAsync(command.NationalNumber, cancellationToken);

        if (!userId.HasValue || !await _identityService.IsUserActiveAsync(userId.Value, cancellationToken))
        {
            return ApiResponse<AdminDto>.Failure(
                "Citizen does not have an active user account in the system.",
                statusCode: 400);
        }

        // 2.1 Verify target user does not possess the SuperAdmin role
        var targetUserRoles = await _identityService.GetUserRolesAsync(userId.Value, cancellationToken);
        if (targetUserRoles.Contains(AppRoles.SuperAdmin))
        {
            return ApiResponse<AdminDto>.Failure(
                "Users with SuperAdmin role cannot be assigned as branch administrators.",
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

        // 3.1 Verify target branch does not already have an active admin
        bool hasActiveAdmin = await _branchAdminValidator.HasActiveBranchAdminAsync(command.BranchId, userId.Value, cancellationToken);
        if (hasActiveAdmin)
        {
            return ApiResponse<AdminDto>.Failure(
                "This branch already has an active administrator. Please relieve or transfer the current administrator first.",
                statusCode: 400);
        }

        // 4. Check if citizen is already assigned as an employee/admin
        var existingEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.UserId == userId.Value, cancellationToken);

        if (existingEmployee != null)
        {
            bool isAlreadyAdmin = targetUserRoles.Contains(AppRoles.Admin);

            if (existingEmployee.BranchId == command.BranchId)
            {
                if (isAlreadyAdmin)
                {
                    return ApiResponse<AdminDto>.Failure(
                        "This citizen is already assigned as an Admin to this branch.",
                        statusCode: 400);
                }

                // Promote existing employee in the same branch to Admin
                if (!existingEmployee.IsActive)
                {
                    existingEmployee.IsActive = true;
                    existingEmployee.LastModifiedAt = DateTime.UtcNow;
                    _context.Employees.Update(existingEmployee);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // Assign Admin role
                await _identityService.AssignUserRolesAsync(userId.Value, new[] { AppRoles.Admin }, cancellationToken);

                // Trigger Domain Event to notify citizen account
                var promotionEvent = new AdminAssignedEvent(userId.Value, branch.BranchName);
                await _adminAssignedEventHandler.HandleAsync(promotionEvent, cancellationToken);

                var promotedAdminDto = new AdminDto
                {
                    EmployeeId = existingEmployee.Id,
                    UserId = userId.Value,
                    NationalNumber = person.NationalNumber,
                    FullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}",
                    EmployeeNumber = existingEmployee.EmployeeNumber,
                    BranchId = branch.Id,
                    BranchName = branch.BranchName,
                    OrganizationName = branch.OrganizationName,
                    IsActive = existingEmployee.IsActive,
                    CreatedAt = existingEmployee.CreatedAt
                };

                return ApiResponse<AdminDto>.Success(
                    promotedAdminDto,
                    message: "Employee promoted to Admin in this branch successfully.");
            }
            else
            {
                if (isAlreadyAdmin)
                {
                    return ApiResponse<AdminDto>.Failure(
                        "This user is already an Admin in another branch. Use Update Admin to transfer the admin to this branch.",
                        statusCode: 400);
                }

                return ApiResponse<AdminDto>.Failure(
                    "This citizen is already assigned as an employee to another branch. Cannot assign them as an admin to a different branch directly.",
                    statusCode: 400);
            }
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

        // 6.1 Trigger Domain Event to notify citizen account
        var adminEvent = new AdminAssignedEvent(userId.Value, branch.BranchName);
        await _adminAssignedEventHandler.HandleAsync(adminEvent, cancellationToken);

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
