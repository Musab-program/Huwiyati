namespace Huwiyati.Application.Requests.Queries;

/// <summary>
/// Query for retrieving a service request by its unique identifier.
/// </summary>
public class GetServiceRequestByIdQuery
{
    /// <summary>
    /// Gets or sets the unique identifier of the requested service request.
    /// </summary>
    public Guid RequestId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetServiceRequestByIdQuery"/> class.
    /// </summary>
    public GetServiceRequestByIdQuery()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetServiceRequestByIdQuery"/> class with the specified request identifier.
    /// </summary>
    /// <param name="requestId">The service request unique identifier.</param>
    public GetServiceRequestByIdQuery(Guid requestId)
    {
        RequestId = requestId;
    }
}
