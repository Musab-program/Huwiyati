namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

public class RemoveDeviceHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public RemoveDeviceHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<ApiResponse<string>> RemoveDeviceAsync(
        RemoveDeviceCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Get UserId by National Number
        var userId = await _identityService.GetUserIdByNationalNumberAsync(command.NationalNumber, cancellationToken);
        if (!userId.HasValue || userId.Value == Guid.Empty)
        {
            return ApiResponse<string>.Failure("User not found.", statusCode: 404);
        }

        // 2. Search for the device in UserDevices table
        var device = await _context.UserDevices
            .FirstOrDefaultAsync(d => d.UserId == userId.Value && d.DeviceIdentifier == command.DeviceIdentifier, cancellationToken);

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