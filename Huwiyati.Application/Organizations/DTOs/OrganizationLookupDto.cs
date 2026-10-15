namespace Huwiyati.Application.Organizations.DTOs;

/// <summary>
/// Represents a lightweight data transfer object for organization lookup dropdowns and references.
/// </summary>
public class OrganizationLookupDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the organization.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the organization.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}