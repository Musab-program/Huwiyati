namespace Huwiyati.Application.Documents.BirthCertificate.Commands;

using Huwiyati.Domain.Enums;

/// <summary>
/// Command payload for updating personal information of a child associated with a birth certificate.
/// </summary>
public class UpdateChildDataCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the birth certificate being updated.
    /// </summary>
    public Guid BirthCertificateId { get; set; }

    /// <summary>
    /// Gets or sets the corrected or updated first name of the child.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the corrected date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the corrected place of birth.
    /// </summary>
    public string PlaceOfBirth { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the corrected gender of the child.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets or sets the corrected blood group.
    /// </summary>
    public BloodGroup BloodGroup { get; set; }

    /// <summary>
    /// Gets or sets the updated governorate of residence.
    /// </summary>
    public string Governorate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated district of residence.
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated residential address details.
    /// </summary>
    public string AddressDetails { get; set; } = string.Empty;
}
