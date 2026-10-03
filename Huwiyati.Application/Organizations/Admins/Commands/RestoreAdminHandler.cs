namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;

using Huwiyati.Domain.Events.Organizations;

public class RestoreAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IDomainEventHandler<AdminRestoredEvent> _adminRestoredEventHandler;

    public RestoreAdminHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IDomainEventHandler<AdminRestoredEvent> adminRestoredEventHandler)
    {
        _context = context;
        _identityService = identityService;
        _adminRestoredEventHandler = adminRestoredEventHandler;
    }

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
