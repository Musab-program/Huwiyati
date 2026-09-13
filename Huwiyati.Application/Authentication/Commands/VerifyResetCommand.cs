namespace Huwiyati.Application.Authentication.Commands;

public class VerifyResetCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}