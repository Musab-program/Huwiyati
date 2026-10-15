namespace Huwiyati.Application.Documents.Passport.Commands;

/// <summary>
/// Command payload for adding a border travel movement entry linked to an active passport.
/// </summary>
public class AddTravelRecordCommand
{
    /// <summary>
    /// Gets or sets the official passport number used during travel.
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the border crossing or airport branch identifier.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the destination or origin foreign country.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the entry date.
    /// </summary>
    public DateOnly EntryDate { get; set; }

    /// <summary>
    /// Gets or sets the optional departure exit date.
    /// </summary>
    public DateOnly? ExitDate { get; set; }
}
