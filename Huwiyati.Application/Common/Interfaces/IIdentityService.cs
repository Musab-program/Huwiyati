using Huwiyati.Application.Common.Models;
using Huwiyati.Domain.Enums;

namespace Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Provides identity management services including user creation, authentication, role assignment, and status updates.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Creates a new user account linked to a Person record in the Civil Registry.
    /// </summary>
    Task<CreateUserResultModel> CreateUserAsync(
        Guid personId,
        string nationalNumber,
        string email,
        string phoneNumber,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user already exists with the given email address.
    /// </summary>
    Task<bool> UserExistsWithEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user already exists with the given National Number.
    /// </summary>
    Task<bool> UserExistsWithNationalNumberAsync(string nationalNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates user credentials and retrieves basic login information if successful.
    /// </summary>
    Task<UserLoginInfoModel?> CheckUserExistAsync(
        string nationalNumber,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the unique User ID associated with a National Number.
    /// </summary>
    Task<Guid?> GetUserIdByNationalNumberAsync(string nationalNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves contact details (Email, Phone) and linked Person ID for a given User ID.
    /// </summary>
    Task<(Guid PersonId, string Email, string PhoneNumber)?> GetUserContactAndPersonIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a user's account status is currently Active.
    /// </summary>
    Task<bool> IsUserActiveAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Assigns specified roles to a user account.
    /// </summary>
    Task<bool> AssignUserRolesAsync(Guid userId, string[] roles, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a specified role from a user account.
    /// </summary>
    Task<bool> RemoveUserRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets a user's password with a new password string.
    /// </summary>
    Task<bool> ResetPasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the list of roles assigned to a specified user.
    /// </summary>
    Task<IList<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes a user's account status (Active, Deactivated, Suspended).
    /// </summary>
    Task<bool> ChangeAccountStatusAsync(Guid userId, AccountStatus status, CancellationToken cancellationToken = default);
}
