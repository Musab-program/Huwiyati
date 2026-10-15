namespace Huwiyati.Application.Organizations.Branches.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Handles restoring (reactivating) a previously deactivated organization branch.
/// </summary>
public class RestoreBranchHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestoreBranchHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public RestoreBranchHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Reactivates an organization branch asynchronously.
    /// </summary>
    /// <param name="id">The branch identifier.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> indicating whether the restoration succeeded.</returns>
    public async Task<ApiResponse<bool>> RestoreBranchAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var branch = await _context.OrganizationBranches
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        if (branch == null)
        {
            return ApiResponse<bool>.Failure(
                "Organization branch was not found.",
                statusCode: 404);
        }

        // Restore Branch (Reactivate)
        branch.IsActive = true;
        branch.LastModifiedAt = DateTime.UtcNow;

        _context.OrganizationBranches.Update(branch);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Success(true, message: "Organization branch restored (activated) successfully.");
    }
}
