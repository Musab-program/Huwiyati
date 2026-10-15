namespace Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Provides access to the currently authenticated user's context in the running request.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the unique identifier of the currently authenticated user, or null if unauthenticated.
    /// </summary>
    string? UserId { get; }
}
