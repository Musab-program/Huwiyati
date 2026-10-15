namespace Huwiyati.Application.Organizations.Admins.DTOs;

/// <summary>
/// Represents data transfer object containing administrative employee details.
/// </summary>
public class AdminDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the employee record.
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated user account.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the national identification number of the administrator.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full legal name of the administrator.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the administrator.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the administrator.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the official employee identification code.
    /// </summary>
    public string EmployeeNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the organization branch where the admin is assigned.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the assigned branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the parent organization.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the administrator account is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the employee record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
