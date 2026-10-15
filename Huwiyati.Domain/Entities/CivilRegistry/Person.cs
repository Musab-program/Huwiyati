namespace Huwiyati.Domain.Entities.CivilRegistry;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;

/// <summary>
/// Represents the central civil registry entity for an individual citizen or resident.
/// Holds biographical, demographic, physical identification attributes, and links to official documents.
/// </summary>
public class Person : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique 11-digit national identity number issued by the civil status authority.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the given first name in Arabic script.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the father's name in Arabic script.
    /// </summary>
    public string FatherName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the grandfather's name in Arabic script.
    /// </summary>
    public string GrandfatherName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the family or tribal surname in Arabic script.
    /// </summary>
    public string FamilyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the official Gregorian birth date.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the city, hospital, or locality of birth.
    /// </summary>
    public string PlaceOfBirth { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the biological sex and legal gender.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets or sets the legal citizenship nationality (e.g., Yemeni).
    /// </summary>
    public string Nationality { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current legal marital status.
    /// </summary>
    public MaritalStatus MaritalStatus { get; set; }

    /// <summary>
    /// Gets or sets the registered governorate of residence.
    /// </summary>
    public string Governorate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the administrative district within the governorate.
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed physical street, neighborhood, or residential address.
    /// </summary>
    public string AddressDetails { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the storage relative URL or filename of the citizen's biometric portrait photo.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the medically verified blood group and Rh factor.
    /// </summary>
    public BloodGroup BloodGroup { get; set; }

    /// <summary>
    /// Gets or sets the civil vitality and legal standing status of the person.
    /// </summary>
    public PersonStatus PersonStatus { get; set; }

    /// <summary>
    /// Gets or sets the historical and active national identification cards issued to this person.
    /// </summary>
    public ICollection<NationalIdCard> NationalIdCards { get; set; } = new List<NationalIdCard>();

    /// <summary>
    /// Gets or sets the historical and active international passports issued to this person.
    /// </summary>
    public ICollection<Passport> Passports { get; set; } = new List<Passport>();
}
