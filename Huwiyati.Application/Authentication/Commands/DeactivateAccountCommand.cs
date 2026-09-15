namespace Huwiyati.Application.Authentication.Commands;

public class DeactivateAccountCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
