namespace Huwiyati.Application.Authentication.Queries;

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;

/// <summary>
/// Handles retrieving all registered hardware devices for the authenticated user.
/// </summary>
public class GetUserDevicesHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserDevicesHandler"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="identityService">The identity management service.</param>
    public GetUserDevicesHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Retrieves all registered devices associated with the authenticated user.
    /// </summary>
    /// <param name="userClaims">The claims principal of the authenticated user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the list of user devices.</returns>
    public async Task<ApiResponse<List<UserDeviceDto>>> HandleAsync(
        ClaimsPrincipal userClaims,
        CancellationToken cancellationToken = default)
    {
        var userId = _identityService.GetUserIdFromClaims(userClaims);
        if (userId == Guid.Empty)
        {
            return ApiResponse<List<UserDeviceDto>>.Failure("User security context is invalid or missing.", statusCode: 401);
        }

        var devices = await _context.UserDevices
            .AsNoTracking()
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.LastLogin ?? d.CreatedAt)
            .Select(d => new UserDeviceDto
            {
                Id = d.Id,
                DeviceIdentifier = d.DeviceIdentifier,
                DeviceName = d.DeviceName,
                OperatingSystem = d.OperatingSystem,
                IsTrusted = d.IsTrusted,
                LastLogin = d.LastLogin,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<UserDeviceDto>>.Success(devices, message: "User devices retrieved successfully.");
    }
}
