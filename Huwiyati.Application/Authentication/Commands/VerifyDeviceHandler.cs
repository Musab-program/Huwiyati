namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;
using Huwiyati.Domain.Events.Authentication;

/// <summary>
/// Handles new device OTP verification, marks the hardware device as trusted, triggers login success events, and issues authentication tokens.
/// </summary>
public class VerifyDeviceHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly IDomainEventHandler<NewDeviceLoginSuccessEvent> _newDeviceLoginSuccessEventHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyDeviceHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="tokenService">The token generation service.</param>
    /// <param name="newDeviceLoginSuccessEventHandler">The domain event handler for device login success notifications.</param>
    public VerifyDeviceHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        ITokenService tokenService,
        IDomainEventHandler<NewDeviceLoginSuccessEvent> newDeviceLoginSuccessEventHandler)
    {
        _context = context;
        _identityService = identityService;
        _tokenService = tokenService;
        _newDeviceLoginSuccessEventHandler = newDeviceLoginSuccessEventHandler;
    }

    /// <summary>
    /// Validates the OTP for an unrecognized device, establishes device trust, and issues a valid authentication token.
    /// </summary>
    /// <param name="command">The device verification command parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the logged-in user result and JWT tokens.</returns>
    public async Task<ApiResponse<LoginResultDto>> VerifyDeviceAsync(VerifyDeviceCommand command, CancellationToken cancellationToken = default)
    {
        var userId = await _identityService.GetUserIdByNationalNumberAsync(command.NationalNumber, cancellationToken);
        if (!userId.HasValue || userId.Value == Guid.Empty)
        {
            return ApiResponse<LoginResultDto>.Failure("Invalid National Number or code.", statusCode: 400);
        }

        // 1. Verify OTP code
        var verificationCode = await _context.VerificationCodes
            .Where(vc => vc.UserId == userId.Value && !vc.IsUsed)
            .OrderByDescending(vc => vc.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (verificationCode == null || verificationCode.Code != command.Code || verificationCode.ExpirationTime < DateTime.UtcNow)
        {
            return ApiResponse<LoginResultDto>.Failure("Invalid or expired verification code.", statusCode: 400);
        }

        // 2. Mark device as trusted
        var device = await _context.UserDevices
            .FirstOrDefaultAsync(d => d.UserId == userId.Value && d.DeviceIdentifier == command.DeviceIdentifier, cancellationToken);

        if (device == null)
        {
            return ApiResponse<LoginResultDto>.Failure("Device record not found.", statusCode: 404);
        }

        device.IsTrusted = true;
        device.LastLogin = DateTime.UtcNow;
        verificationCode.IsUsed = true;

        await _context.SaveChangesAsync(cancellationToken);

        // Trigger NewDeviceLoginSuccessEvent
        var newDeviceLoginSuccessEvent = new NewDeviceLoginSuccessEvent(userId.Value, device.DeviceName);
        await _newDeviceLoginSuccessEventHandler.HandleAsync(newDeviceLoginSuccessEvent, cancellationToken);

        // 3. Get Citizen Name and Issue JWT Token
        var person = await _context.Persons.FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);
        var fullName = person != null ? $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim() : command.NationalNumber;
        var roles = await _identityService.GetUserRolesAsync(userId.Value, cancellationToken);

        var tokenModel = _tokenService.GenerateToken(userId.Value,
                                                     command.NationalNumber,
                                                     fullName,
                                                     "Active",
                                                     roles);

        return ApiResponse<LoginResultDto>.Success(new LoginResultDto
        {
            UserId = userId.Value,
            AccessToken = tokenModel.Token,
            Expiration = tokenModel.Expiration,
            NationalNumber = command.NationalNumber,
            FullName = fullName,
            AccountStatus = "Active",
            RequiresDeviceVerification = false
        }, message: "Device verified successfully. Login completed.");
    }
}