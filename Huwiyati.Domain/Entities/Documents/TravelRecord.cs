namespace Huwiyati.Domain.Entities.Documents;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.Organizations;

/// <summary>
/// Represents an international border crossing entry or exit travel record associated with a passport.
/// </summary>
public class TravelRecord : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the passport used for travel.
    /// </summary>
    public Guid PassportId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the port of entry/exit or issuing border branch.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the destination or origin country of travel.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the recorded date of entry into the country.
    /// </summary>
    public DateOnly EntryDate { get; set; }

    /// <summary>
    /// Gets or sets the recorded date of exit from the country, if departed.
    /// </summary>
    public DateOnly? ExitDate { get; set; }

    /// <summary>
    /// Gets or sets the passport navigation entity.
    /// </summary>
    public Passport Passport { get; set; } = null!;

    /// <summary>
    /// Gets or sets the border branch or station navigation entity.
    /// </summary>
    public OrganizationBranch IssuingBranch { get; set; } = null!;
}
