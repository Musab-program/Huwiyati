namespace Huwiyati.Application.Requests.Queries;

using Huwiyati.Domain.Enums;

public class GetCitizenServiceRequestsQuery
{
    public RequestStatus? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
