namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;
using Huwiyati.Domain.Enums;
using Huwiyati.Domain.Entities.Authentication;

public class RegisterHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public RegisterHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

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

        // 2. Validate Date of Birth matching
        if (person.DateOfBirth != command.DateOfBirth)
        {
            return ApiResponse<RegisterResultDto>.Failure(
                "Provided Date of Birth does not match official Civil Registry records.",
                statusCode: 400);
        }

        // 3. Check if an account already exists for this National Number
        var nationalNumberExists = await _identityService.UserExistsWithNationalNumberAsync(person.NationalNumber, cancellationToken);
        if (nationalNumberExists)
        {
            return ApiResponse<RegisterResultDto>.Failure(
                "An account with this National Number already exists.",
                statusCode: 400);
        }

        // 4. Check if an account already exists for this email
        var emailExists = await _identityService.UserExistsWithEmailAsync(command.Email, cancellationToken);
        if (emailExists)
        {
            return ApiResponse<RegisterResultDto>.Failure(
                "An account with this email address already exists.",
                statusCode: 400);
        }

        // 4. Create ApplicationUser account using IIdentityService
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

        // 5. Generate 6-digit OTP verification code valid for 5 minutes
        var otpCode = new Random().Next(100000, 999999).ToString();
        var verificationCode = new VerificationCode
        {
            UserId = createUserResult.UserId,
            Code = otpCode,
            ExpirationTime = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.VerificationCodes.AddAsync(verificationCode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 6. Return success response with UserId and PendingActivation status
        return ApiResponse<RegisterResultDto>.Success(
            new RegisterResultDto
            {
                UserId = createUserResult.UserId,
                NationalNumber = person.NationalNumber,
                AccountStatus = AccountStatus.PendingActivation.ToString()
            },
            message: $"Account created successfully. Verification OTP code generated: {otpCode}");
    }
}
