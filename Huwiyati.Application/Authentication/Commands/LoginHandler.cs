namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;
using Huwiyati.Domain.Enums;
using Huwiyati.Domain.Entities.Authentication;

public class LoginHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public LoginHandler(IApplicationDbContext context, IIdentityService identityService, ITokenService tokenService)
    {
        _context = context;
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<LoginResultDto>> LoginHandlerAsync(
        LoginCommand command,
        CancellationToken cancellation = default)
    {
        // 1. Check if the user exists and validate credentials
        var userLoginInfo = await _identityService.CheckUserExistAsync(command.NationalNumber, command.Password, cancellation);

        if (userLoginInfo == null || !userLoginInfo.Succeeded)
        {
            return ApiResponse<LoginResultDto>.Failure(
                "Invalid National Number or Password.",
                statusCode: 401);
        }

        // 2. Check if Account is Suspended or Deactivated
        if (userLoginInfo.AccountStatus == AccountStatus.Suspended.ToString() ||
            userLoginInfo.AccountStatus == AccountStatus.Deactivated.ToString())
        {
            return ApiResponse<LoginResultDto>.Failure(
                "Your account has been suspended or deactivated. Please contact support.",
                statusCode: 403);
        }

        // 3. Get Citizen Details from Civil Registry (Persons)
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.Id == userLoginInfo.PersonId, cancellation);
        var fullName = person != null
            ? $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim()
            : command.NationalNumber;

        // 4. Device Check (Mandatory)
        var device = await _context.UserDevices
            .FirstOrDefaultAsync(d => d.UserId == userLoginInfo.UserId && d.DeviceIdentifier == command.DeviceIdentifier, cancellation);

        // Case A: Device exists and is trusted -> Direct Login
        if (device != null && device.IsTrusted)
        {
            device.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellation);

            var tokenModel = _tokenService.GenerateToken(userLoginInfo.UserId,
                                                         command.NationalNumber,
                                                         fullName,
                                                         userLoginInfo.AccountStatus,
                                                         userLoginInfo.Roles);

            return ApiResponse<LoginResultDto>.Success(
                new LoginResultDto
                {
                    UserId = userLoginInfo.UserId,
                    AccessToken = tokenModel.Token,
                    Expiration = tokenModel.Expiration,
                    NationalNumber = command.NationalNumber,
                    FullName = fullName,
                    AccountStatus = userLoginInfo.AccountStatus,
                    RequiresDeviceVerification = false
                },
                message: "Login successful.");
        }

        // Case B: Untrusted or New Device -> Register device & generate OTP code
        if (device == null)
        {
            device = new UserDevice
            {
                UserId = userLoginInfo.UserId,
                DeviceIdentifier = command.DeviceIdentifier,
                DeviceName = string.IsNullOrWhiteSpace(command.DeviceName) ? "Unknown Device" : command.DeviceName,
                OperatingSystem = string.IsNullOrWhiteSpace(command.OperatingSystem) ? "Unknown OS" : command.OperatingSystem,
                IsTrusted = false,
                CreatedAt = DateTime.UtcNow
            };
            await _context.UserDevices.AddAsync(device, cancellation);
        }

        var otpCode = new Random().Next(100000, 999999).ToString();
        var verificationCode = new VerificationCode
        {
            UserId = userLoginInfo.UserId,
            Code = otpCode,
            ExpirationTime = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.VerificationCodes.AddAsync(verificationCode, cancellation);
        await _context.SaveChangesAsync(cancellation);

        return ApiResponse<LoginResultDto>.Success(
            new LoginResultDto
            {
                UserId = userLoginInfo.UserId,
                AccessToken = string.Empty,
                NationalNumber = command.NationalNumber,
                FullName = fullName,
                AccountStatus = userLoginInfo.AccountStatus,
                RequiresDeviceVerification = true
            },
            message: $"New device detected. Verification OTP code generated: {otpCode}");
    }
}
