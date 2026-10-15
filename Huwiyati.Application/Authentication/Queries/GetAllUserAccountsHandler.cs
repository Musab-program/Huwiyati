namespace Huwiyati.Application.Authentication.Queries;

using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;

/// <summary>
/// Query handler returning a complete roster of registered user accounts across the system for super administration.
/// </summary>
public class GetAllUserAccountsHandler
{
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllUserAccountsHandler"/> class.
    /// </summary>
    /// <param name="identityService">The identity management service.</param>
    public GetAllUserAccountsHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    /// Retrieves all registered user accounts asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the list of user accounts.</returns>
    public async Task<ApiResponse<List<RegisteredUserAccountDto>>> GetAllUserAccountsAsync(
        CancellationToken cancellationToken = default)
    {
        var accounts = await _identityService.GetAllUserAccountsAsync(cancellationToken);
        return ApiResponse<List<RegisteredUserAccountDto>>.Success(accounts);
    }
}
