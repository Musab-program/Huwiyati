namespace Huwiyati.Application.Authentication.DTOs;

using System;

/// <summary>
/// Data transfer object returned upon successful or conditional authentication attempt.
/// </summary>
public class LoginResultDto
{
    /// <summary>
    /// Gets or sets the authenticated user identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the generated JWT access token string.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC expiration date and time of the access token.
    /// </summary>
    public DateTime Expiration { get; set; }

    /// <summary>
    /// Gets or sets the refresh token string used to obtain new access tokens.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC expiration date and time of the refresh token.
    /// </summary>
    public DateTime RefreshTokenExpiration { get; set; }

    /// <summary>
    /// Gets or sets the user's national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account status string.
    /// </summary>
    public string AccountStatus { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether login requires two-factor verification for an unrecognized device.
    /// </summary>
    public bool RequiresDeviceVerification { get; set; }
}
