namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Admins.DTOs;
using Huwiyati.Domain.Constants;

using Huwiyati.Domain.Events.Organizations;

/// <summary>
/// Handles updating an administrator's branch assignment and re-generating employee numbers as necessary.
/// </summary>
public class UpdateAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmployeeNumberGenerator _employeeNumberGenerator;
    private readonly IDomainEventHandler<AdminUpdatedEvent> _adminUpdatedEventHandler;
    private readonly IBranchAdminValidator _branchAdminValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAdminHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="employeeNumberGenerator">The generator for standardized employee numbers.</param>
    /// <param name="adminUpdatedEventHandler">The event handler for admin update domain events.</param>
    /// <param name="branchAdminValidator">The validator ensuring single admin per branch.</param>
    public UpdateAdminHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmployeeNumberGenerator employeeNumberGenerator,
        IDomainEventHandler<AdminUpdatedEvent> adminUpdatedEventHandler,
        IBranchAdminValidator branchAdminValidator)
    {
        _context = context;
        _identityService = identityService;
        _employeeNumberGenerator = employeeNumberGenerator;
        _adminUpdatedEventHandler = adminUpdatedEventHandler;
        _branchAdminValidator = branchAdminValidator;
    }

    /// <summary>
    /// Updates an administrator's branch assignment asynchronously.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="command">The update admin command payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the updated <see cref="AdminDto"/>.</returns>
    public async Task<ApiResponse<AdminDto>> UpdateAdminAsync(
        Guid employeeId,
        UpdateAdminCommand command,
        CancellationToken cancellationToken = default)
    {
        var emp = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);

        if (emp == null)
        {
            return ApiResponse<AdminDto>.Failure(
                "Admin record was not found.",
                statusCode: 404);
        }

        // Verify that the employee record is active
        if (!emp.IsActive)
        {
            return ApiResponse<AdminDto>.Failure(
                "Cannot update an inactive administrator record.",
                statusCode: 400);
        }

        // Verify that the user account is active
        if (!await _identityService.IsUserActiveAsync(emp.UserId, cancellationToken))
        {
            return ApiResponse<AdminDto>.Failure(
                "The user account associated with this administrator is not active.",
                statusCode: 400);
        }

        // Verify that the administrator is not deceased in Civil Registry
        var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(emp.UserId, cancellationToken);
        var person = contactInfo.HasValue
            ? await _context.Persons.FirstOrDefaultAsync(p => p.Id == contactInfo.Value.PersonId, cancellationToken)
            : null;

        if (person != null && person.PersonStatus == Huwiyati.Domain.Enums.PersonStatus.Deceased)
        {
            return ApiResponse<AdminDto>.Failure(
                "Cannot update a deceased administrator record.",
                statusCode: 400);
        }

        // Verify that this employee has the Admin role
        var roles = await _identityService.GetUserRolesAsync(emp.UserId, cancellationToken);
        if (!roles.Contains(AppRoles.Admin))
        {
            return ApiResponse<AdminDto>.Failure(
                "The specified employee is not an Admin.",
                statusCode: 400);
        }

        // Check if admin is already assigned to the requested branch
        if (emp.BranchId == command.BranchId)
        {
            return ApiResponse<AdminDto>.Failure(
                "The admin is already assigned to this branch.",
                statusCode: 400);
        }

        var newBranchData = await _context.OrganizationBranches
            .Where(b => b.Id == command.BranchId && b.IsActive)
            .Select(b => new
            {
                Branch = b,
                OrganizationName = b.Organization.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (newBranchData == null)
        {
            return ApiResponse<AdminDto>.Failure(
                "The target organization branch was not found or is inactive.",
                statusCode: 404);
        }

        // Verify target branch does not already have an active admin
        bool targetBranchHasAdmin = await _branchAdminValidator.HasActiveBranchAdminAsync(command.BranchId, emp.UserId, cancellationToken);
        if (targetBranchHasAdmin)
        {
            return ApiResponse<AdminDto>.Failure(
                "The target branch already has an active administrator. Please relieve the administrator of that branch first.",
                statusCode: 400);
        }

        // Generate new employee number for the new branch
        string newEmployeeNumber = await _employeeNumberGenerator.GenerateEmployeeNumberAsync(command.BranchId, cancellationToken);

        emp.BranchId = command.BranchId;
        emp.EmployeeNumber = newEmployeeNumber;
        emp.LastModifiedAt = DateTime.UtcNow;

        _context.Employees.Update(emp);
        await _context.SaveChangesAsync(cancellationToken);

        // 4.1 Trigger Domain Event to notify user
        var updateEvent = new AdminUpdatedEvent(emp.UserId, newBranchData.Branch.BranchName);
        await _adminUpdatedEventHandler.HandleAsync(updateEvent, cancellationToken);

        var adminDto = new AdminDto
        {
            EmployeeId = emp.Id,
            UserId = emp.UserId,
            NationalNumber = person?.NationalNumber ?? string.Empty,
            FullName = person != null ? $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}" : string.Empty,
            Email = contactInfo?.Email ?? string.Empty,
            PhoneNumber = contactInfo?.PhoneNumber ?? string.Empty,
            EmployeeNumber = emp.EmployeeNumber,
            BranchId = emp.BranchId,
            BranchName = newBranchData.Branch.BranchName,
            OrganizationName = newBranchData.OrganizationName,
            IsActive = emp.IsActive,
            CreatedAt = emp.CreatedAt
        };

        return ApiResponse<AdminDto>.Success(adminDto, message: "Admin branch updated successfully.");
    }
}
