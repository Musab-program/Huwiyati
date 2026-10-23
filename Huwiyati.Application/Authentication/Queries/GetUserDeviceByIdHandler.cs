namespace Huwiyati.Application.Authentication.Queries;

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;

/// <summary>
/// Handles retrieving details for a specific device owned by the authenticated user.
/// </summary>
public class GetUserDeviceByIdHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserDeviceByIdHandler"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="identityService">The identity management service.</param>
    public GetUserDeviceByIdHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Retrieves a single device record by ID belonging to the authenticated user.
    /// </summary>
    /// <param name="deviceId">The primary key ID of the device.</param>
    /// <param name="userClaims">The claims principal of the authenticated user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the device details if found and owned by the user.</returns>
    public async Task<ApiResponse<UserDeviceDto>> HandleAsync(
        Guid deviceId,
        ClaimsPrincipal userClaims,
        CancellationToken cancellationToken = default)
    {
        var userId = _identityService.GetUserIdFromClaims(userClaims);
        if (userId == Guid.Empty)
        {
            return ApiResponse<UserDeviceDto>.Failure("User security context is invalid or missing.", statusCode: 401);
        }

        var device = await _context.UserDevices
            .AsNoTracking()
            .Where(d => d.Id == deviceId && d.UserId == userId)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (device == null)
        {
            return ApiResponse<UserDeviceDto>.Failure("Device record not found.", statusCode: 404);
        }

        return ApiResponse<UserDeviceDto>.Success(device, message: "Device details retrieved successfully.");
    }
}
