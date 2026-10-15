namespace Huwiyati.Infrastructure.Identity;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Models;

/// <summary>
/// Service responsible for generating JWT access tokens and cryptographically secure refresh tokens.
/// </summary>
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenService"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration root containing JWT parameters.</param>
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generates a JSON Web Token (JWT) along with a cryptographically random refresh token.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="nationalNumber">The national identification number (username).</param>
    /// <param name="fullName">The full name of the user.</param>
    /// <param name="accountStatus">The user's account status.</param>
    /// <param name="roles">The roles assigned to the user.</param>
    /// <param name="organizationId">Optional organization identifier for staff members.</param>
    /// <param name="branchId">Optional branch identifier for staff members.</param>
    /// <returns>A <see cref="GenerateTokenModel"/> containing token strings and expiry timestamps.</returns>
    public GenerateTokenModel GenerateToken(
        Guid userId,
        string nationalNumber,
        string fullName,
        string accountStatus,
        IEnumerable<string> roles,
        Guid? organizationId = null,
        Guid? branchId = null)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"]!;
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expiryMinutes = double.Parse(jwtSettings["ExpiryMinutes"] ?? "15");
        var refreshExpiryHours = double.Parse(jwtSettings["RefreshTokenExpiryHours"] ?? "3");

        var expire = DateTime.UtcNow.AddMinutes(expiryMinutes);
        var refreshExpire = DateTime.UtcNow.AddHours(refreshExpiryHours);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Name, nationalNumber),
            new("fullName", fullName),
            new("accountStatus", accountStatus),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Add optional organization and branch claims if staff context is present
        if (organizationId.HasValue && organizationId.Value != Guid.Empty)
        {
            claims.Add(new Claim("OrganizationId", organizationId.Value.ToString()));
        }

        if (branchId.HasValue && branchId.Value != Guid.Empty)
        {
            claims.Add(new Claim("BranchId", branchId.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expire,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(securityToken);

        // Generate cryptographically secure random refresh token bytes
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var refreshTokenString = Convert.ToBase64String(randomBytes);

        return new GenerateTokenModel
        {
            Token = tokenString,
            Expiration = expire,
            RefreshToken = refreshTokenString,
            RefreshTokenExpiration = refreshExpire
        };
    }
}
