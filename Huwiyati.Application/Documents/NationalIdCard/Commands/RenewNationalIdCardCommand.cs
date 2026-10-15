namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Huwiyati.Domain.Enums;

/// <summary>
/// Command payload for renewing an existing National ID Card and optionally updating citizen personal information.
/// </summary>
public class RenewNationalIdCardCommand
{
    /// <summary>
    /// Gets or sets the unique 11-digit national number identifying the citizen.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional updated marital status.
    /// </summary>
    public MaritalStatus? MaritalStatus { get; set; }

    /// <summary>
    /// Gets or sets the optional updated governorate.
    /// </summary>
    public string? Governorate { get; set; }

    /// <summary>
    /// Gets or sets the optional updated district.
    /// </summary>
    public string? District { get; set; }

    /// <summary>
    /// Gets or sets optional updated detailed address information.
    /// </summary>
    public string? AddressDetails { get; set; }

    /// <summary>
    /// Gets or sets the optional updated biometric photo URL.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the civil registry branch identifier where the renewal is processed.
    /// </summary>
    public Guid IssuingBranchId { get; set; }
}
