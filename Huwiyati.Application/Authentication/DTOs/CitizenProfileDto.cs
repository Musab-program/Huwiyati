namespace Huwiyati.Application.Authentication.DTOs;

/// <summary>
/// Data transfer object representing the full personal and demographic profile of an authenticated citizen,
/// along with holding status for key identity documents.
/// </summary>
public class CitizenProfileDto
{
    /// <summary>
    /// Gets or sets the unique Identity user identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the linked civil registry person identifier.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique national identification number.
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
    /// Gets or sets the recorded date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the place of birth.
    /// </summary>
    public string PlaceOfBirth { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gender (e.g. "Male", "Female").
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the nationality.
    /// </summary>
    public string Nationality { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the marital status (e.g. "Single", "Married", "Divorced", "Widowed").
    /// </summary>
    public string MaritalStatus { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the blood group code (e.g. "A_Positive", "O_Positive").
    /// </summary>
    public string BloodGroup { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the governorate (province) of residence.
    /// </summary>
    public string Governorate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the district of residence.
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the residential address details.
    /// </summary>
    public string AddressDetails { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL or path to the citizen's official profile photograph.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the registered email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the registered contact phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current account status description (e.g. "Active", "Pending").
    /// </summary>
    public string AccountStatus { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the citizen has an active national ID card.
    /// </summary>
    public bool HasActiveNationalIdCard { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the citizen holds an active passport.
    /// </summary>
    public bool HasActivePassport { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the citizen has an active family card.
    /// </summary>
    public bool HasActiveFamilyCard { get; set; }
}
