namespace Huwiyati.Application.Authentication.DTOs;

using System;

public class LoginResultDto
{
    public Guid UserId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; set; }
    public string NationalNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string AccountStatus { get; set; } = string.Empty;
    public bool RequiresDeviceVerification { get; set; }
}
