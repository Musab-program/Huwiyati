namespace Huwiyati.Application.Organizations.Employees.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

using Huwiyati.Domain.Events.Organizations;

/// <summary>
/// Handles activating an existing employee account within the admin's branch.
/// </summary>
public class ActivateEmployeeHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IDomainEventHandler<EmployeeActivatedEvent> _employeeActivatedEventHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivateEmployeeHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="employeeActivatedEventHandler">The event handler for employee activated domain events.</param>
    public ActivateEmployeeHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IDomainEventHandler<EmployeeActivatedEvent> employeeActivatedEventHandler)
    {
        _context = context;
        _identityService = identityService;
        _employeeActivatedEventHandler = employeeActivatedEventHandler;
    }

    /// <summary>
    /// Activates an employee record in the admin's branch asynchronously.
    /// </summary>
    /// <param name="currentAdminUserId">The user identifier of the executing administrator.</param>
    /// <param name="employeeId">The employee identifier to activate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> indicating whether the operation succeeded.</returns>
    public async Task<ApiResponse<bool>> ActivateEmployeeAsync(
        Guid currentAdminUserId,
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        // 1. Resolve Admin's BranchId from token UserId
        var adminBranchId = await _context.Employees
            .Where(e => e.UserId == currentAdminUserId && e.IsActive)
            .Select(e => (Guid?)e.BranchId)
            .FirstOrDefaultAsync(cancellationToken);

        if (!adminBranchId.HasValue)
        {
            return ApiResponse<bool>.Failure("Current logged-in admin is not associated with an active branch.", statusCode: 403);
        }

        // 2. Get employee from database ensuring branch scoping
        var emp = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeId && e.BranchId == adminBranchId.Value, cancellationToken);

        if (emp == null)
        {
            return ApiResponse<bool>.Failure("Employee was not found in your branch.", statusCode: 404);
        }

        if (emp.IsActive)
        {
            return ApiResponse<bool>.Failure("Employee is already active.", statusCode: 400);
        }

        // 2.1 Verify user account is active
        if (!await _identityService.IsUserActiveAsync(emp.UserId, cancellationToken))
        {
            return ApiResponse<bool>.Failure("The user account associated with this employee is not active.", statusCode: 400);
        }

        // 2.2 Verify employee is not deceased in Civil Registry
        var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(emp.UserId, cancellationToken);
        var person = contactInfo.HasValue && contactInfo.Value.PersonId != Guid.Empty
            ? await _context.Persons.FirstOrDefaultAsync(p => p.Id == contactInfo.Value.PersonId, cancellationToken)
            : null;

        if (person != null && person.PersonStatus == Huwiyati.Domain.Enums.PersonStatus.Deceased)
        {
            return ApiResponse<bool>.Failure("Cannot activate a deceased employee.", statusCode: 400);
        }

        // 3. Update IsActive status to true
        emp.IsActive = true;
        _context.Employees.Update(emp);
        await _context.SaveChangesAsync(cancellationToken);

        // 3.1 Trigger Domain Event to notify employee user account
        var branchName = await _context.OrganizationBranches
            .Where(b => b.Id == emp.BranchId)
            .Select(b => b.BranchName)
            .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

        var activateEvent = new EmployeeActivatedEvent(emp.UserId, branchName);
        await _employeeActivatedEventHandler.HandleAsync(activateEvent, cancellationToken);

        return ApiResponse<bool>.Success(true, message: "Employee activated successfully.");
    }
}
