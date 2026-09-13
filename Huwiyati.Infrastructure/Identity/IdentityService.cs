namespace Huwiyati.Infrastructure.Identity;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Models;
using Huwiyati.Domain.Constants;
using Huwiyati.Domain.Enums;
using Microsoft.AspNetCore.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> UserExistsWithEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }

    public async Task<bool> UserExistsWithNationalNumberAsync(string nationalNumber, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(nationalNumber);
        return user != null;
    }

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
        // Assign the "Citizen" role to the newly created user as default
        await _userManager.AddToRoleAsync(user, AppRoles.Citizen);

        return new CreateUserResultModel
        {
            Succeeded = true,
            UserId = user.Id
        };
    }

    public async Task<UserLoginInfoModel?> CheckUserExistAsync(string nationalNumber, string password, CancellationToken cancellationToken = default)
    {
        // 1. Search for ApplicationUser by National Number (UserName)
        var user = await _userManager.FindByNameAsync(nationalNumber);
        if (user == null)
        {
            return null;
        }

        // 2. Validate Password
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
        {
            return null;
        }

        //3. Get user roles (if needed for further processing)
        var roles = await _userManager.GetRolesAsync(user);

        // 4. Return user login details
        return new UserLoginInfoModel
        {
            Succeeded = true,
            UserId = user.Id,
            PersonId = user.PersonId,
            AccountStatus = user.Status.ToString(),
            Roles = roles.ToList()
        };
    }

    public async Task<Guid?> GetUserIdByNationalNumberAsync(string nationalNumber, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(nationalNumber);
        return user?.Id;
    }

    public async Task<bool> ResetPasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;
        var removeResult = await _userManager.RemovePasswordAsync(user);
        if (!removeResult.Succeeded && user.PasswordHash != null) return false;
        var addResult = await _userManager.AddPasswordAsync(user, newPassword);
        return addResult.Succeeded;

    }

    public async Task<IList<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // 1. Search for ApplicationUser by Id
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return new List<string>();
        }

        // 2. Get user roles
        return await _userManager.GetRolesAsync(user);
    }


}
