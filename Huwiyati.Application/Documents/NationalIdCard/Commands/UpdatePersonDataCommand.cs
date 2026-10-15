namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Huwiyati.Domain.Enums;

/// <summary>
/// Command payload for updating personal demographic and profile details of a registered citizen.
/// </summary>
public class UpdatePersonDataCommand
{
    /// <summary>
    /// Gets or sets the 11-digit national number of the citizen being updated.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated father's name.
    /// </summary>
    public string FatherName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated grandfather's name.
    /// </summary>
    public string GrandfatherName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated family or surname.
    /// </summary>
    public string FamilyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the updated place of birth.
    /// </summary>
    public string PlaceOfBirth { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated marital status.
    /// </summary>
    public MaritalStatus MaritalStatus { get; set; }

    /// <summary>
    /// Gets or sets the updated governorate of residence.
    /// </summary>
    public string Governorate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated district of residence.
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets updated detailed address information.
    /// </summary>
    public string AddressDetails { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional updated biometric photo URL.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the updated blood group.
    /// </summary>
    public BloodGroup BloodGroup { get; set; }
}
