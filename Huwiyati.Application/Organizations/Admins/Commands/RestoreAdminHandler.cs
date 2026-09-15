namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;

public class RestoreAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public RestoreAdminHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
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

        return ApiResponse<bool>.Success(true, message: "Admin role has been restored successfully.");
    }
}
