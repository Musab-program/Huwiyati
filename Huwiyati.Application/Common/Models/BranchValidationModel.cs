namespace Huwiyati.Application.Common.Models;

/// <summary>
/// Encapsulates the verification outcome and metadata of an organization branch validation.
/// </summary>
public class BranchValidationModel
{
    /// <summary>
    /// Gets or sets a value indicating whether the branch meets all validation criteria.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the descriptive error message if validation fails.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the corresponding HTTP status code (e.g. 200, 400, 404).
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the validated branch unique identifier.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the validated branch name.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parent organization identifier.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the parent organization name.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;
}
