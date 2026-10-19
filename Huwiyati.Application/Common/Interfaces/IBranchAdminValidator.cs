namespace Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Defines validation operations to ensure branch administrator exclusivity and operational constraints.
/// </summary>
public interface IBranchAdminValidator
{
    /// <summary>
    /// Checks if the specified branch already has an active administrator assigned.
    /// </summary>
    /// <param name="branchId">The branch identifier.</param>
    /// <param name="excludeUserId">Optional user identifier to exclude from the check.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if an active administrator exists; otherwise, false.</returns>
    Task<bool> HasActiveBranchAdminAsync(
        Guid branchId,
        Guid? excludeUserId = null,
        CancellationToken cancellationToken = default);
}
