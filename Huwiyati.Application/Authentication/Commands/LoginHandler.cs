namespace Huwiyati.Application.Authentication.Commands;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;
using Huwiyati.Domain.Constants;
using Huwiyati.Domain.Enums;
using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Events.Authentication;

public class LoginHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IDomainEventHandler<NewDeviceLoginAttemptEvent> _newDeviceLoginAttemptEventHandler;

    public LoginHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        ITokenService tokenService,
        IEmailService emailService,
        IDomainEventHandler<NewDeviceLoginAttemptEvent> newDeviceLoginAttemptEventHandler)
    {
        _context = context;
        _identityService = identityService;
        _tokenService = tokenService;
        _emailService = emailService;
        _newDeviceLoginAttemptEventHandler = newDeviceLoginAttemptEventHandler;
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

        // 2. Check if Account is Suspended
        if (userLoginInfo.AccountStatus == AccountStatus.Suspended.ToString())
        {
            return ApiResponse<LoginResultDto>.Failure(
                "Your account has been suspended by administration. Please visit a service center to reactivate your account.",
                statusCode: 403);
        }

        // 3. Check if Account is Deactivated
        if (userLoginInfo.AccountStatus == AccountStatus.Deactivated.ToString())
        {
            return ApiResponse<LoginResultDto>.Failure(
                "Your account is currently deactivated. Please request a reactivation code via /api/v1/account/request-otp.",
                statusCode: 403);
        }

        // 4. Validate that the user actually possesses the requested role in Identity
        if (!userLoginInfo.Roles.Contains(command.RequestedRole))
        {
            return ApiResponse<LoginResultDto>.Failure(
                $"Access Denied: You do not possess the '{command.RequestedRole}' role.",
                statusCode: 403);
        }

        Guid? validatedOrganizationId = null;
        Guid? validatedBranchId = null;

        // 5. Role & Branch Context Validation for Employee and Admin (WITHOUT using Include)
        if (command.RequestedRole == AppRoles.Employee || command.RequestedRole == AppRoles.Admin)
        {
            if (!command.BranchId.HasValue || command.BranchId.Value == Guid.Empty)
            {
                return ApiResponse<LoginResultDto>.Failure(
                    "Branch ID is required for staff authentication.",
                    statusCode: 400);
            }

            var employeeRecord = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UserId == userLoginInfo.UserId
                                       && e.BranchId == command.BranchId.Value
                                       && e.IsActive, cancellation);

            if (employeeRecord == null)
            {
                return ApiResponse<LoginResultDto>.Failure(
                    "Access Denied: You are not assigned as an active staff member for this branch.",
                    statusCode: 403);
            }

            var branch = await _context.OrganizationBranches
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == command.BranchId.Value && b.IsActive, cancellation);

            if (branch == null)
            {
                return ApiResponse<LoginResultDto>.Failure(
                    "Access Denied: The specified branch is inactive or does not exist.",
                    statusCode: 403);
            }

            validatedBranchId = branch.Id;
            validatedOrganizationId = branch.OrganizationId;
        }

        // 6. Get Citizen Details from Civil Registry (Persons)
        var person = await _context.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == userLoginInfo.PersonId, cancellation);
        var fullName = person != null
            ? $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim()
            : command.NationalNumber;

        // 7. Device Check (Mandatory)
        var device = await _context.UserDevices
            .FirstOrDefaultAsync(d => d.UserId == userLoginInfo.UserId && d.DeviceIdentifier == command.DeviceIdentifier, cancellation);

        var activeRoles = new List<string> { command.RequestedRole };

        // Case A: Device exists and is trusted -> Direct Login
        if (device != null && device.IsTrusted)
        {
            device.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellation);

            var tokenModel = _tokenService.GenerateToken(
                userLoginInfo.UserId,
                command.NationalNumber,
                fullName,
                userLoginInfo.AccountStatus,
                activeRoles,
                validatedOrganizationId,
                validatedBranchId);

            var refreshTokenEntity = new RefreshToken
            {
                UserId = userLoginInfo.UserId,
                Token = tokenModel.RefreshToken,
                JwtId = Guid.NewGuid().ToString(),
                ExpiryDate = tokenModel.RefreshTokenExpiration,
                IsUsed = false,
                IsRevoked = false
            };
            await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return ApiResponse<LoginResultDto>.Success(
                new LoginResultDto
                {
                    UserId = userLoginInfo.UserId,
                    AccessToken = tokenModel.Token,
                    Expiration = tokenModel.Expiration,
                    RefreshToken = tokenModel.RefreshToken,
                    RefreshTokenExpiration = tokenModel.RefreshTokenExpiration,
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

        // Trigger NewDeviceLoginAttemptEvent
        var newDeviceLoginAttemptEvent = new NewDeviceLoginAttemptEvent(userLoginInfo.UserId, device.DeviceName);
        await _newDeviceLoginAttemptEventHandler.HandleAsync(newDeviceLoginAttemptEvent, cancellation);

        // Send OTP via Email Service securely
        var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(userLoginInfo.UserId, cancellation);
        if (contactInfo.HasValue && !string.IsNullOrWhiteSpace(contactInfo.Value.Email))
        {
            var emailSent = await _emailService.SendOtpEmailAsync(
                contactInfo.Value.Email,
                "رمز التحقق - تسجيل الدخول من جهاز جديد",
                otpCode,
                "التحقق من تسجيل الدخول من جهاز جديد",
                cancellation);

            if (!emailSent)
            {
                return ApiResponse<LoginResultDto>.Failure(
                    "Failed to send device verification OTP email. Please try again later.",
                    statusCode: 500);
            }
        }

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
            message: "New device detected. A verification OTP code has been sent to your registered email.");
    }
}
