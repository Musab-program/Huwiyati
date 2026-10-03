namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;

using Huwiyati.Domain.Events.Organizations;

public class RemoveAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IDomainEventHandler<AdminRemovedEvent> _adminRemovedEventHandler;

    public RemoveAdminHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IDomainEventHandler<AdminRemovedEvent> adminRemovedEventHandler)
    {
        _context = context;
        _identityService = identityService;
        _adminRemovedEventHandler = adminRemovedEventHandler;
    }

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
