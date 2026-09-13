using Huwiyati.Application.Common.Models;

namespace Huwiyati.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<CreateUserResultModel> CreateUserAsync(
        Guid personId,
        string nationalNumber,
        string email,
        string phoneNumber,
        string password,
        CancellationToken cancellationToken = default);

    Task<bool> UserExistsWithEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> UserExistsWithNationalNumberAsync(string nationalNumber, CancellationToken cancellationToken = default);

    Task<UserLoginInfoModel?> CheckUserExistAsync(
    string nationalNumber,
    string password,
    CancellationToken cancellationToken = default);

    Task<Guid?> GetUserIdByNationalNumberAsync(string nationalNumber, CancellationToken cancellationToken = default);

    Task<bool> ResetPasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken = default);

    Task<IList<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
}
