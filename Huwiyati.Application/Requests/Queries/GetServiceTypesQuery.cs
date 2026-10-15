namespace Huwiyati.Application.Requests.Queries;

/// <summary>
/// Query for retrieving active service types, optionally filtered by organization.
/// </summary>
public class GetServiceTypesQuery
{
    /// <summary>
    /// Gets or sets an optional organization identifier filter.
    /// </summary>
    public Guid? OrganizationId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetServiceTypesQuery"/> class.
    /// </summary>
    public GetServiceTypesQuery()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetServiceTypesQuery"/> class with the specified organization identifier.
    /// </summary>
    /// <param name="organizationId">The organization unique identifier filter.</param>
    public GetServiceTypesQuery(Guid? organizationId)
    {
        OrganizationId = organizationId;
    }
}
