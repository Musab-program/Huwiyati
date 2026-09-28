namespace Huwiyati.Application.Requests.Queries;

using Huwiyati.Domain.Enums;

public class GetBranchServiceRequestsQuery
{
    public RequestStatus? Status { get; set; }
    public string? SearchKeyword { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
