namespace Huwiyati.Application.Authentication.Commands;

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Handles requests to revoke device trust so that subsequent logins from that device require multi-factor OTP verification.
/// </summary>
public class RemoveDeviceHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveDeviceHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="identityService">The identity management service.</param>
    public RemoveDeviceHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Revokes trust for a registered user device belonging to the authenticated user.
    /// </summary>
    /// <param name="userClaims">Current authenticated user claims from JWT.</param>
    /// <param name="command">The remove device command details containing the device identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the operation status message.</returns>
    public async Task<ApiResponse<string>> RemoveDeviceAsync(
        ClaimsPrincipal userClaims,
        RemoveDeviceCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Resolve UserId securely from JWT claims
        var userId = _identityService.GetUserIdFromClaims(userClaims);
        if (userId == Guid.Empty)
        {
            return ApiResponse<string>.Failure("User security context is invalid or missing.", statusCode: 401);
        }

        if (string.IsNullOrWhiteSpace(command.DeviceIdentifier))
        {
            return ApiResponse<string>.Failure("Device identifier is required.", statusCode: 400);
        }

        // 2. Search for the device in UserDevices table belonging specifically to this authenticated user
        var device = await _context.UserDevices
            .FirstOrDefaultAsync(d => d.UserId == userId && d.DeviceIdentifier == command.DeviceIdentifier, cancellationToken);

        if (device == null)
        {
            return ApiResponse<string>.Failure("Device record not found.", statusCode: 404);
        }

        // 3. Set device as untrusted
        device.IsTrusted = false;
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<string>.Success(
            "Device untrusted and removed successfully.",
            message: "The device has been untrusted. OTP verification will be required on next login.");
    }
}
