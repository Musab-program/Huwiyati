namespace Huwiyati.Application.Organizations.Branches.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Handles soft-deleting (deactivating) an organization branch.
/// </summary>
public class DeleteBranchHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteBranchHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public DeleteBranchHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Soft-deletes an organization branch by deactivating it.
    /// </summary>
    /// <param name="id">The branch identifier.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> indicating whether the deactivation succeeded.</returns>
    public async Task<ApiResponse<bool>> DeleteBranchAsync(
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

        // Soft Delete: Deactivate branch to preserve historical references
        branch.IsActive = false;
        branch.LastModifiedAt = DateTime.UtcNow;

        _context.OrganizationBranches.Update(branch);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Success(true, message: "Organization branch deleted successfully.");
    }
}
