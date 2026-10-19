namespace Huwiyati.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;

/// <summary>
/// Service responsible for validating administrator assignment rules per branch.
/// </summary>
public class BranchAdminValidator : IBranchAdminValidator
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="BranchAdminValidator"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity management service.</param>
    public BranchAdminValidator(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Checks if the specified branch already has an active administrator assigned.
    /// </summary>
    /// <param name="branchId">The branch identifier.</param>
    /// <param name="excludeUserId">Optional user identifier to exclude from the check.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if an active administrator exists; otherwise, false.</returns>
    public async Task<bool> HasActiveBranchAdminAsync(
        Guid branchId,
        Guid? excludeUserId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Employees
            .Where(e => e.BranchId == branchId && e.IsActive);

        if (excludeUserId.HasValue)
        {
            query = query.Where(e => e.UserId != excludeUserId.Value);
        }

        var employeeUserIds = await query
            .Select(e => e.UserId)
            .ToListAsync(cancellationToken);

        foreach (var userId in employeeUserIds)
        {
            var roles = await _identityService.GetUserRolesAsync(userId, cancellationToken);
            if (roles.Contains(AppRoles.Admin))
            {
                return true;
            }
        }

        return false;
    }
}
