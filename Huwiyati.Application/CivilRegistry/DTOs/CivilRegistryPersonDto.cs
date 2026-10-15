namespace Huwiyati.Application.CivilRegistry.DTOs;

using Huwiyati.Domain.Enums;

/// <summary>
/// Data transfer object representing a citizen's official civil registry master record for administrative review.
/// </summary>
public class CivilRegistryPersonDto
{
    /// <summary>
    /// Gets or sets the civil registry unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the complete quadruple full name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the father's name.
    /// </summary>
    public string FatherName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the grandfather's name.
    /// </summary>
    public string GrandfatherName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the family or surname.
    /// </summary>
    public string FamilyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the official date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the official place of birth.
    /// </summary>
    public string PlaceOfBirth { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gender enum.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets or sets the nationality.
    /// </summary>
    public string Nationality { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the marital status enum.
    /// </summary>
    public MaritalStatus MaritalStatus { get; set; }

    /// <summary>
    /// Gets or sets the blood group enum.
    /// </summary>
    public BloodGroup BloodGroup { get; set; }

    /// <summary>
    /// Gets or sets the governorate (province) of registry.
    /// </summary>
    public string Governorate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the district of registry.
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address details.
    /// </summary>
    public string AddressDetails { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the citizen's archival portrait photo.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the biological life status (Alive or Deceased).
    /// </summary>
    public PersonStatus PersonStatus { get; set; }
}
