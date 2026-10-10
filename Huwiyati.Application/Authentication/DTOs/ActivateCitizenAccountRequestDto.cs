namespace Huwiyati.Application.Authentication.DTOs;

// Request body for staff account activation endpoint
public class ActivateCitizenAccountRequestDto
{
    public string NationalNumber { get; set; } = string.Empty;
}
