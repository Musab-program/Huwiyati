namespace Huwiyati.Infrastructure.Identity;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Models;
using Huwiyati.Domain.Constants;
using Huwiyati.Domain.Enums;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Implementation of IIdentityService providing identity management operations backed by ASP.NET Core Identity.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Checks if a user already exists with the specified email.
    /// </summary>
    public async Task<bool> UserExistsWithEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }

    /// <summary>
    /// Checks if a user already exists with the specified National Number.
    /// </summary>
    public async Task<bool> UserExistsWithNationalNumberAsync(string nationalNumber, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(nationalNumber);
        return user != null;
    }

    /// <summary>
    /// Creates a new user account linked to a Person ID and assigns the default Citizen role.
    /// </summary>
    public async Task<CreateUserResultModel> CreateUserAsync(
        Guid personId,
        string nationalNumber,
        string email,
        string phoneNumber,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            PersonId = personId,
            UserName = nationalNumber,
            Email = email,
            PhoneNumber = phoneNumber,
            Status = AccountStatus.PendingActivation,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return new CreateUserResultModel
            {
                Succeeded = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }
        await _userManager.AddToRoleAsync(user, AppRoles.Citizen);

        return new CreateUserResultModel
        {
            Succeeded = true,
            UserId = user.Id
        };
    }

    /// <summary>
    /// Validates user credentials (National Number and Password) and returns login metadata.
    /// </summary>
    public async Task<UserLoginInfoModel?> CheckUserExistAsync(string nationalNumber, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(nationalNumber);
        if (user == null)
        {
            return null;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new UserLoginInfoModel
        {
            Succeeded = true,
            UserId = user.Id,
            PersonId = user.PersonId,
            AccountStatus = user.Status.ToString(),
            Roles = roles.ToList()
        };
    }

    /// <summary>
    /// Finds the User ID for a user identified by their National Number.
    /// </summary>
    public async Task<Guid?> GetUserIdByNationalNumberAsync(string nationalNumber, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(nationalNumber);
        return user?.Id;
    }

    /// <summary>
    /// Retrieves contact details (Email, Phone) and Person ID for a specified user.
    /// </summary>
    public async Task<(Guid PersonId, string Email, string PhoneNumber)?> GetUserContactAndPersonIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return null;
        var email = !string.IsNullOrWhiteSpace(user.Email) ? user.Email : string.Empty;
        return (user.PersonId, email, user.PhoneNumber ?? string.Empty);
    }

    /// <summary>
    /// Verifies if a user's account status is currently Active.
    /// </summary>
    public async Task<bool> IsUserActiveAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user != null && user.Status == AccountStatus.Active;
    }

    /// <summary>
    /// Assigns a set of roles to a specified user account.
    /// </summary>
    public async Task<bool> AssignUserRolesAsync(Guid userId, string[] roles, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        foreach (var role in roles)
        {
            if (!await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        return true;
    }

    /// <summary>
    /// Removes a specific role from a user account.
    /// </summary>
    public async Task<bool> RemoveUserRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        if (await _userManager.IsInRoleAsync(user, role))
        {
            await _userManager.RemoveFromRoleAsync(user, role);
        }

        return true;
    }

    /// <summary>
    /// Resets the password for a specified user ID.
    /// </summary>
    public async Task<bool> ResetPasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;
        var removeResult = await _userManager.RemovePasswordAsync(user);
        if (!removeResult.Succeeded && user.PasswordHash != null) return false;
        var addResult = await _userManager.AddPasswordAsync(user, newPassword);
        return addResult.Succeeded;
    }

    /// <summary>
    /// Gets all roles assigned to a specified user.
    /// </summary>
    public async Task<IList<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return new List<string>();
        }

        return await _userManager.GetRolesAsync(user);
    }

    /// <summary>
    /// Updates the AccountStatus enum value (Active, Deactivated, Suspended) for a user.
    /// </summary>
    public async Task<bool> ChangeAccountStatusAsync(Guid userId, AccountStatus status, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        user.Status = status;
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}
