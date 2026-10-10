namespace Huwiyati.Application.Authentication.Queries;

using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;

// Query handler returning list of all registered online accounts for SuperAdmin
public class GetAllUserAccountsHandler
{
    private readonly IIdentityService _identityService;

    public GetAllUserAccountsHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ApiResponse<List<RegisteredUserAccountDto>>> GetAllUserAccountsAsync(
        CancellationToken cancellationToken = default)
    {
        var accounts = await _identityService.GetAllUserAccountsAsync(cancellationToken);
        return ApiResponse<List<RegisteredUserAccountDto>>.Success(accounts);
    }
}
