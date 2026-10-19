namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;
using Huwiyati.Domain.Enums;
using Huwiyati.Domain.Entities.Authentication;

/// <summary>
/// Handles citizen account registration by verifying personal data against the Civil Registry, provisioning user records, and generating activation OTPs.
/// </summary>
public class RegisterHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="emailService">The email dispatching service.</param>
    public RegisterHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmailService emailService)
    {
        _context = context;
        _identityService = identityService;
        _emailService = emailService;
    }

    /// <summary>
    /// Validates citizen identity against the civil registry and creates a pending user account.
    /// </summary>
    /// <param name="command">The registration parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the registration outcome.</returns>
    public async Task<ApiResponse<RegisterResultDto>> CreateAccountAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Search for the citizen by National Number in Civil Registry
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<RegisterResultDto>.Failure(
                "Citizen with the provided National Number was not found in the Civil Registry.",
                statusCode: 404);
        }

        // 1.1 Verify citizen is not deceased
        if (person.PersonStatus == PersonStatus.Deceased)
        {
            return ApiResponse<RegisterResultDto>.Failure(
                "Cannot register an account for a citizen registered as deceased in Civil Registry.",
                statusCode: 400);
        }

        // 2. Validate Date of Birth matching
        if (person.DateOfBirth != command.DateOfBirth)
        {
            return ApiResponse<RegisterResultDto>.Failure(
                "Provided Date of Birth does not match official Civil Registry records.",
                statusCode: 400);
        }

        // 3. Check if an account already exists for this National Number
        var existingUserId = await _identityService.GetUserIdByNationalNumberAsync(person.NationalNumber, cancellationToken);
        Guid targetUserId;

        if (existingUserId.HasValue && existingUserId.Value != Guid.Empty)
        {
            var accountDetails = await _identityService.GetUserAccountDetailsByPersonIdAsync(person.Id, cancellationToken);
            if (accountDetails != null && accountDetails.Status == AccountStatus.Active)
            {
                return ApiResponse<RegisterResultDto>.Failure(
                    "An account with this National Number already exists and is active. Please login.",
                    statusCode: 400);
            }

            // Account exists but not active (e.g. email typo) -> allow updating email/phone/password
            var emailTaken = await _identityService.UserExistsWithEmailAsync(command.Email, cancellationToken);
            if (emailTaken && accountDetails?.Email != command.Email)
            {
                return ApiResponse<RegisterResultDto>.Failure(
                    "An account with this email address already exists.",
                    statusCode: 400);
            }

            var updateSuccess = await _identityService.UpdateUnverifiedAccountAsync(
                existingUserId.Value,
                command.Email,
                command.PhoneNumber,
                command.Password,
                cancellationToken);

            if (!updateSuccess)
            {
                return ApiResponse<RegisterResultDto>.Failure("Failed to update unverified account details.", statusCode: 500);
            }

            targetUserId = existingUserId.Value;

            // Invalidate previous OTP codes
            var oldCodes = await _context.VerificationCodes
                .Where(vc => vc.UserId == targetUserId && !vc.IsUsed)
                .ToListAsync(cancellationToken);

            foreach (var c in oldCodes)
            {
                c.IsUsed = true;
            }
        }
        else
        {
            // 4. Check if an account already exists for this email
            var emailExists = await _identityService.UserExistsWithEmailAsync(command.Email, cancellationToken);
            if (emailExists)
            {
                return ApiResponse<RegisterResultDto>.Failure(
                    "An account with this email address already exists.",
                    statusCode: 400);
            }

            // 5. Create ApplicationUser account using IIdentityService
            var createUserResult = await _identityService.CreateUserAsync(
                 person.Id,
                 person.NationalNumber,
                 command.Email,
                 command.PhoneNumber,
                 command.Password,
                 cancellationToken);

            if (!createUserResult.Succeeded)
            {
                return ApiResponse<RegisterResultDto>.Failure(
                    "Failed to create user account.",
                    createUserResult.Errors,
                    statusCode: 400);
            }

            targetUserId = createUserResult.UserId;
        }

        // 6. Generate 6-digit OTP verification code valid for 5 minutes
        var otpCode = new Random().Next(100000, 999999).ToString();
        var verificationCode = new VerificationCode
        {
            UserId = targetUserId,
            Code = otpCode,
            ExpirationTime = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.VerificationCodes.AddAsync(verificationCode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 7. Send OTP code via Email Service securely
        var emailSent = await _emailService.SendOtpEmailAsync(
            command.Email,
            "رمز التحقق - تفعيل حساب هويتي",
            otpCode,
            "تفعيل الحساب",
            cancellationToken);

        if (!emailSent)
        {
            return ApiResponse<RegisterResultDto>.Failure(
                "Failed to send verification OTP email. Please check email service configuration.",
                statusCode: 500);
        }

        // 8. Return secure success response without exposing OTP code
        return ApiResponse<RegisterResultDto>.Success(
            new RegisterResultDto
            {
                UserId = targetUserId,
                NationalNumber = person.NationalNumber,
                AccountStatus = AccountStatus.PendingActivation.ToString()
            },
            message: "Account created successfully. A verification OTP code has been sent to your registered email.");
    }
}
