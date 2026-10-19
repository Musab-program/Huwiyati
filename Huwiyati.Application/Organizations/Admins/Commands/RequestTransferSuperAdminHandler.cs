namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;
using Huwiyati.Domain.Entities.Authentication;

/// <summary>
/// Handles requesting transfer of SuperAdmin privileges by verifying identity and sending an OTP verification code.
/// </summary>
public class RequestTransferSuperAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestTransferSuperAdminHandler"/> class.
    /// </summary>
    public RequestTransferSuperAdminHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmailService emailService)
    {
        _context = context;
        _identityService = identityService;
        _emailService = emailService;
    }

    /// <summary>
    /// Verifies current SuperAdmin credentials and dispatches an OTP verification code asynchronously.
    /// </summary>
    public async Task<ApiResponse<bool>> RequestTransferAsync(
        Guid currentSuperAdminUserId,
        RequestTransferSuperAdminCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify caller has SuperAdmin role
        var callerRoles = await _identityService.GetUserRolesAsync(currentSuperAdminUserId, cancellationToken);
        if (!callerRoles.Contains(AppRoles.SuperAdmin))
        {
            return ApiResponse<bool>.Failure(
                "Only an active SuperAdmin can initiate role transfer.",
                statusCode: 403);
        }

        // 2. Verify caller contact info and email
        var callerContact = await _identityService.GetUserContactAndPersonIdAsync(currentSuperAdminUserId, cancellationToken);
        if (!callerContact.HasValue || string.IsNullOrWhiteSpace(callerContact.Value.Email))
        {
            return ApiResponse<bool>.Failure(
                "SuperAdmin account does not have a registered email address for verification.",
                statusCode: 400);
        }

        // 3. Verify caller national number matches authenticated user
        var callerPerson = await _context.Persons
            .FirstOrDefaultAsync(p => p.Id == callerContact.Value.PersonId, cancellationToken);

        if (callerPerson == null || callerPerson.NationalNumber != command.SuperAdminNationalNumber)
        {
            return ApiResponse<bool>.Failure(
                "The provided national number does not match the authenticated SuperAdmin.",
                statusCode: 400);
        }

        // 4. Verify current SuperAdmin password
        var credentialCheck = await _identityService.CheckUserExistAsync(
            command.SuperAdminNationalNumber,
            command.CurrentPassword,
            cancellationToken);

        if (credentialCheck == null)
        {
            return ApiResponse<bool>.Failure(
                "Invalid password. Identity verification failed.",
                statusCode: 400);
        }

        // 5. Generate secure 6-digit OTP code
        string otpCode = Random.Shared.Next(100000, 999999).ToString();

        // 6. Invalidate any existing unused codes for this user
        var existingCodes = await _context.VerificationCodes
            .Where(vc => vc.UserId == currentSuperAdminUserId && !vc.IsUsed)
            .ToListAsync(cancellationToken);

        foreach (var code in existingCodes)
        {
            code.IsUsed = true;
        }

        // 7. Persist new verification code with 10-minute validity
        var verification = new VerificationCode
        {
            UserId = currentSuperAdminUserId,
            Code = otpCode,
            ExpirationTime = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false,
            Attempts = 0,
            CreatedAt = DateTime.UtcNow
        };

        await _context.VerificationCodes.AddAsync(verification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 8. Dispatch OTP to the registered email address
        await _emailService.SendOtpEmailAsync(
            callerContact.Value.Email,
            "Transfer SuperAdmin Privileges Security Code",
            otpCode,
            "Transferring SuperAdmin ownership",
            cancellationToken);

        return ApiResponse<bool>.Success(
            true,
            message: "Security verification code has been dispatched to your registered email address.");
    }
}
