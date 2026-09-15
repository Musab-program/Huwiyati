namespace Huwiyati.Application.Organizations.Branches.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

public class RestoreBranchHandler
{
    private readonly IApplicationDbContext _context;

    public RestoreBranchHandler(IApplicationDbContext context)
    {
        _context = context;
    }

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
