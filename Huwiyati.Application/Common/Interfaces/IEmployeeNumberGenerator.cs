namespace Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Defines generator operations for creating unique administrative employee badge numbers.
/// </summary>
public interface IEmployeeNumberGenerator
{
    /// <summary>
    /// Generates a unique employee identification badge number scoped to a specific branch.
    /// </summary>
    /// <param name="branchId">The identifier of the branch where the employee is assigned.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A unique employee badge number string.</returns>
    Task<string> GenerateEmployeeNumberAsync(Guid branchId, CancellationToken cancellationToken = default);
}
