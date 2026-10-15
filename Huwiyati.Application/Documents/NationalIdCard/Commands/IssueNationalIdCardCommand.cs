namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Huwiyati.Domain.Enums;

/// <summary>
/// Command payload for issuing a new National ID Card for an existing or new citizen record.
/// </summary>
public class IssueNationalIdCardCommand
{
    /// <summary>
    /// Gets or sets the optional identifier of an existing citizen person record.
    /// </summary>
    public Guid? PersonId { get; set; }

    /// <summary>
    /// Gets or sets the first name of the citizen when creating a new person record.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the father's name of the citizen when creating a new person record.
    /// </summary>
    public string? FatherName { get; set; }

    /// <summary>
    /// Gets or sets the grandfather's name of the citizen when creating a new person record.
    /// </summary>
    public string? GrandfatherName { get; set; }

    /// <summary>
    /// Gets or sets the family or surname of the citizen when creating a new person record.
    /// </summary>
    public string? FamilyName { get; set; }

    /// <summary>
    /// Gets or sets the date of birth of the citizen when creating a new person record.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the place of birth of the citizen when creating a new person record.
    /// </summary>
    public string? PlaceOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the gender of the citizen when creating a new person record.
    /// </summary>
    public Gender? Gender { get; set; }

    /// <summary>
    /// Gets or sets the nationality of the citizen when creating a new person record.
    /// </summary>
    public string? Nationality { get; set; }

    /// <summary>
    /// Gets or sets the marital status of the citizen.
    /// </summary>
    public MaritalStatus? MaritalStatus { get; set; }

    /// <summary>
    /// Gets or sets the governorate of residence.
    /// </summary>
    public string? Governorate { get; set; }

    /// <summary>
    /// Gets or sets the district of residence.
    /// </summary>
    public string? District { get; set; }

    /// <summary>
    /// Gets or sets detailed address information.
    /// </summary>
    public string? AddressDetails { get; set; }

    /// <summary>
    /// Gets or sets the biometric photo URL.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the blood group of the citizen.
    /// </summary>
    public BloodGroup? BloodGroup { get; set; }

    /// <summary>
    /// Gets or sets the civil registry branch identifier where the card is issued.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets an optional explicit issue date; defaults to today if omitted.
    /// </summary>
    public DateOnly? IssueDate { get; set; }
}