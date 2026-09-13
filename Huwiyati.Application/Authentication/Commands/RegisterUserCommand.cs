namespace Huwiyati.Application.Authentication.Commands;

public class RegisterUserCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
