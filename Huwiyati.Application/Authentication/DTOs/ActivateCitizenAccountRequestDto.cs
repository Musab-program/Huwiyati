namespace Huwiyati.Application.Authentication.DTOs;

/// <summary>
/// Data transfer object containing request parameters for staff-initiated citizen account activation.
/// </summary>
public class ActivateCitizenAccountRequestDto
{
    /// <summary>
    /// Gets or sets the national identification number of the citizen whose account is being activated.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;
}
