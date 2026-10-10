namespace Huwiyati.Application.Authentication.Commands;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Models;
using Huwiyati.Domain.Entities.Authentication;

public record RefreshTokenCommand(string Token, string RefreshToken);

public class RefreshTokenHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IIdentityService _identityService;

    public RefreshTokenHandler(
        IApplicationDbContext context,
        ITokenService tokenService,
        IIdentityService identityService)
    {
        _context = context;
        _tokenService = tokenService;
        _identityService = identityService;
    }

    public async Task<ApiResponse<GenerateTokenModel>> HandleAsync(RefreshTokenCommand request, CancellationToken cancellationToken = default)
    {
        // Step 1: Retrieve the refresh token record from database without AsNoTracking to allow automatic change tracking
        var storedRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == request.RefreshToken, cancellationToken);

        // Step 2: Validate token existence, usage status, and revocation state
        if (storedRefreshToken == null || storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked)
        {
            return ApiResponse<GenerateTokenModel>.Failure("Invalid, used, or revoked refresh token.", statusCode: 400);
        }

        // Step 3: Verify token expiration timestamp
        if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
        {
            return ApiResponse<GenerateTokenModel>.Failure("Refresh token has expired. Please log in again.", statusCode: 400);
        }

        // Step 4: Mark old refresh token as used (EF Core ChangeTracker automatically detects this property modification)
        storedRefreshToken.IsUsed = true;

        // Step 5: Retrieve user contact and linked Person ID
        var userContact = await _identityService.GetUserContactAndPersonIdAsync(storedRefreshToken.UserId, cancellationToken);
        if (userContact == null)
        {
            return ApiResponse<GenerateTokenModel>.Failure("Associated user account was not found.", statusCode: 404);
        }

        // Step 6: Retrieve Person entity to extract real National Number and Full Name
        var person = await _context.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == userContact.Value.PersonId, cancellationToken);

        var fullName = person != null
            ? string.Join(" ", new[] { person.FirstName, person.FatherName, person.GrandfatherName, person.FamilyName }.Where(s => !string.IsNullOrWhiteSpace(s)))
            : string.Empty;
        var nationalNumber = person?.NationalNumber ?? string.Empty;

        // Step 7: Retrieve user roles
        var roles = await _identityService.GetUserRolesAsync(storedRefreshToken.UserId, cancellationToken);

        // Step 8: Generate new AccessToken and new RefreshToken pair (Token Rotation Pattern)
        var newTokenModel = _tokenService.GenerateToken(
            storedRefreshToken.UserId,
            nationalNumber,
            fullName,
            "Active",
            roles);

        // Step 9: Create and persist the new refresh token record in database
        var newRefreshToken = new RefreshToken
        {
            UserId = storedRefreshToken.UserId,
            Token = newTokenModel.RefreshToken,
            JwtId = Guid.NewGuid().ToString(),
            ExpiryDate = newTokenModel.RefreshTokenExpiration,
            IsUsed = false,
            IsRevoked = false
        };

        await _context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);

        // Step 10: Save changes to persist both updated state of old token and insertion of new refresh token
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<GenerateTokenModel>.Success(newTokenModel, "Token refreshed successfully.", statusCode: 200);
    }
}
