namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;

using Huwiyati.Domain.Events.Organizations;

/// <summary>
/// Handles revoking the administrator role from an employee, retaining their regular employee and citizen status.
/// </summary>
public class RemoveAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IDomainEventHandler<AdminRemovedEvent> _adminRemovedEventHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveAdminHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="adminRemovedEventHandler">The event handler for admin revocation domain events.</param>
    public RemoveAdminHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IDomainEventHandler<AdminRemovedEvent> adminRemovedEventHandler)
    {
        _context = context;
        _identityService = identityService;
        _adminRemovedEventHandler = adminRemovedEventHandler;
    }

    /// <summary>
    /// Revokes the administrator role from an employee asynchronously.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> indicating whether the operation succeeded.</returns>
    public async Task<ApiResponse<bool>> RemoveAdminRoleAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var emp = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);

        if (emp == null)
        {
            return ApiResponse<bool>.Failure(
                "Admin record was not found.",
                statusCode: 404);
        }

        // Verify that this employee has the Admin role
        var roles = await _identityService.GetUserRolesAsync(emp.UserId, cancellationToken);
        if (!roles.Contains(AppRoles.Admin))
        {
            return ApiResponse<bool>.Failure(
                "The specified employee is not an Admin.",
                statusCode: 400);
        }

        // Revoke Admin Role from User (User remains an Employee & Citizen)
        await _identityService.RemoveUserRoleAsync(emp.UserId, AppRoles.Admin, cancellationToken);

        // 4.1 Trigger Domain Event to notify user
        var branchName = await _context.OrganizationBranches
            .Where(b => b.Id == emp.BranchId)
            .Select(b => b.BranchName)
            .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

        var removeEvent = new AdminRemovedEvent(emp.UserId, branchName);
        await _adminRemovedEventHandler.HandleAsync(removeEvent, cancellationToken);

        return ApiResponse<bool>.Success(true, message: "Admin role has been removed successfully. User remains as a regular employee.");
    }
}
