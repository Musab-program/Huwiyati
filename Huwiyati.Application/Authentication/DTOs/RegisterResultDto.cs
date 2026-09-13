namespace Huwiyati.Application.Authentication.DTOs;

public class RegisterResultDto
{
    public Guid? UserId { get; set; }
    public string NationalNumber { get; set; } = string.Empty;
    public string AccountStatus { get; set; } = string.Empty;
}
