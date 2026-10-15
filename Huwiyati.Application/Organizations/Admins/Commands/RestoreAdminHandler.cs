namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;

using Huwiyati.Domain.Events.Organizations;

/// <summary>
/// Handles restoring the administrator role to an existing employee.
/// </summary>
public class RestoreAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IDomainEventHandler<AdminRestoredEvent> _adminRestoredEventHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestoreAdminHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="adminRestoredEventHandler">The event handler for admin restored domain events.</param>
    public RestoreAdminHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IDomainEventHandler<AdminRestoredEvent> adminRestoredEventHandler)
    {
        _context = context;
        _identityService = identityService;
        _adminRestoredEventHandler = adminRestoredEventHandler;
    }

    /// <summary>
    /// Restores the administrator role to an employee asynchronously.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> indicating whether the operation succeeded.</returns>
    public async Task<ApiResponse<bool>> RestoreAdminRoleAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var emp = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);

        if (emp == null)
        {
            return ApiResponse<bool>.Failure(
                "Admin / Employee record was not found.",
                statusCode: 404);
        }

        // Re-assign Admin role to user
        await _identityService.AssignUserRolesAsync(emp.UserId, new[] { AppRoles.Admin }, cancellationToken);

        // 3.1 Trigger Domain Event to notify user
        var branchName = await _context.OrganizationBranches
            .Where(b => b.Id == emp.BranchId)
            .Select(b => b.BranchName)
            .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

        var restoreEvent = new AdminRestoredEvent(emp.UserId, branchName);
        await _adminRestoredEventHandler.HandleAsync(restoreEvent, cancellationToken);

        return ApiResponse<bool>.Success(true, message: "Admin role has been restored successfully.");
    }
}
