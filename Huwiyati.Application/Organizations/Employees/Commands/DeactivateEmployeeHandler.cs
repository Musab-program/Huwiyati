namespace Huwiyati.Application.Organizations.Employees.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

using Huwiyati.Domain.Events.Organizations;

/// <summary>
/// Handles deactivating an employee account within the administrator's branch.
/// </summary>
public class DeactivateEmployeeHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IDomainEventHandler<EmployeeDeactivatedEvent> _employeeDeactivatedEventHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeactivateEmployeeHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="employeeDeactivatedEventHandler">The event handler for employee deactivated domain events.</param>
    public DeactivateEmployeeHandler(
        IApplicationDbContext context,
        IDomainEventHandler<EmployeeDeactivatedEvent> employeeDeactivatedEventHandler)
    {
        _context = context;
        _employeeDeactivatedEventHandler = employeeDeactivatedEventHandler;
    }

    /// <summary>
    /// Deactivates an employee record in the admin's branch asynchronously.
    /// </summary>
    /// <param name="currentAdminUserId">The user identifier of the executing administrator.</param>
    /// <param name="employeeId">The employee identifier to deactivate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> indicating whether the operation succeeded.</returns>
    public async Task<ApiResponse<bool>> DeactivateEmployeeAsync(
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

        if (!emp.IsActive)
        {
            return ApiResponse<bool>.Failure("Employee is already deactivated.", statusCode: 400);
        }

        // 3. Update IsActive status to false
        emp.IsActive = false;
        _context.Employees.Update(emp);
        await _context.SaveChangesAsync(cancellationToken);

        // 3.1 Trigger Domain Event to notify employee user account
        var branchName = await _context.OrganizationBranches
            .Where(b => b.Id == emp.BranchId)
            .Select(b => b.BranchName)
            .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

        var deactivateEvent = new EmployeeDeactivatedEvent(emp.UserId, branchName);
        await _employeeDeactivatedEventHandler.HandleAsync(deactivateEvent, cancellationToken);

        return ApiResponse<bool>.Success(true, message: "Employee deactivated successfully.");
    }
}
