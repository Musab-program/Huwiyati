namespace Huwiyati.Application.Requests.Queries;

using Huwiyati.Domain.Enums;

/// <summary>
/// Query parameters for retrieving paginated service requests submitted by the authenticated citizen.
/// </summary>
public class GetCitizenServiceRequestsQuery
{
    /// <summary>
    /// Gets or sets optional status filter for the requests.
    /// </summary>
    public RequestStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the 1-based page number for pagination. Default is 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size for pagination. Default is 10.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
