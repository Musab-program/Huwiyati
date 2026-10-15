namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;
using Huwiyati.Domain.Enums;
using Huwiyati.Domain.Events.Authentication;

/// <summary>
/// Handles account reactivation by verifying user credentials and OTP, switching status to Active, and generating access tokens.
/// </summary>
public class ReactivateAccountHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly IDomainEventHandler<AccountReactivatedEvent> _accountReactivatedEventHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactivateAccountHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="tokenService">The token generation service.</param>
    /// <param name="accountReactivatedEventHandler">The domain event handler for reactivation notifications.</param>
    public ReactivateAccountHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        ITokenService tokenService,
        IDomainEventHandler<AccountReactivatedEvent> accountReactivatedEventHandler)
    {
        _context = context;
        _identityService = identityService;
        _tokenService = tokenService;
        _accountReactivatedEventHandler = accountReactivatedEventHandler;
    }

    /// <summary>
    /// Reactivates an account and authenticates the user session upon successful validation.
    /// </summary>
    /// <param name="command">The reactivation command containing credentials and OTP code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the logged-in user result and JWT tokens.</returns>
    public async Task<ApiResponse<LoginResultDto>> ReactivateAccountAsync(
        ReactivateAccountCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify credentials
        var userLoginInfo = await _identityService.CheckUserExistAsync(command.NationalNumber, command.Password, cancellationToken);
        if (userLoginInfo == null || !userLoginInfo.Succeeded)
        {
            return ApiResponse<LoginResultDto>.Failure("Invalid National Number or Password.", statusCode: 401);
        }

        // 2. Verify OTP code
        var verificationCode = await _context.VerificationCodes
            .Where(vc => vc.UserId == userLoginInfo.UserId && !vc.IsUsed)
            .OrderByDescending(vc => vc.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (verificationCode == null || verificationCode.Code != command.Code || verificationCode.ExpirationTime < DateTime.UtcNow)
        {
            return ApiResponse<LoginResultDto>.Failure("Invalid or expired verification code.", statusCode: 400);
        }

        // 3. Mark OTP code as used and update Account Status to Active
        verificationCode.IsUsed = true;
        await _identityService.ChangeAccountStatusAsync(userLoginInfo.UserId, AccountStatus.Active, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 4. Trigger AccountReactivatedEvent
        var accountReactivatedEvent = new AccountReactivatedEvent(userLoginInfo.UserId);
        await _accountReactivatedEventHandler.HandleAsync(accountReactivatedEvent, cancellationToken);

        // 5. Get Citizen Name and Issue Token
        var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == userLoginInfo.PersonId, cancellationToken);
        var fullName = person != null ? $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim() : command.NationalNumber;
        var roles = await _identityService.GetUserRolesAsync(userLoginInfo.UserId, cancellationToken);

        var tokenModel = _tokenService.GenerateToken(userLoginInfo.UserId, command.NationalNumber, fullName, "Active", roles);

        return ApiResponse<LoginResultDto>.Success(new LoginResultDto
        {
            UserId = userLoginInfo.UserId,
            AccessToken = tokenModel.Token,
            Expiration = tokenModel.Expiration,
            NationalNumber = command.NationalNumber,
            FullName = fullName,
            AccountStatus = "Active",
            RequiresDeviceVerification = false
        }, message: "Account reactivated successfully. Login completed.");
    }
}
