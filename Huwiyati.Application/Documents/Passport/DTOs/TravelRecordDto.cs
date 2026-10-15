namespace Huwiyati.Application.Documents.Passport.DTOs;

/// <summary>
/// Represents data transfer object containing border crossing or travel movement details.
/// </summary>
public class TravelRecordDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the travel record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the passport used for travel.
    /// </summary>
    public Guid PassportId { get; set; }

    /// <summary>
    /// Gets or sets the passport number used for travel.
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique person identifier of the traveler.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the full legal name of the traveler.
    /// </summary>
    public string PersonFullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the national number of the traveler.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the port or border branch identifier recording the movement.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the port or border issuing branch.
    /// </summary>
    public string IssuingBranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the destination or origin foreign country.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of arrival or departure entry.
    /// </summary>
    public DateOnly EntryDate { get; set; }

    /// <summary>
    /// Gets or sets the optional date of exit or subsequent departure.
    /// </summary>
    public DateOnly? ExitDate { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the record was created in the system.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
