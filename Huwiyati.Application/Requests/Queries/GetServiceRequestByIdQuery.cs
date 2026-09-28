namespace Huwiyati.Application.Requests.Queries;

public class GetServiceRequestByIdQuery
{
    public Guid RequestId { get; set; }

    public GetServiceRequestByIdQuery()
    {
    }

    public GetServiceRequestByIdQuery(Guid requestId)
    {
        RequestId = requestId;
    }
}
