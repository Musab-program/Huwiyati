namespace Huwiyati.Application.Requests.Queries;

public class GetServiceTypesQuery
{
    public Guid? OrganizationId { get; set; }

    public GetServiceTypesQuery()
    {
    }

    public GetServiceTypesQuery(Guid? organizationId)
    {
        OrganizationId = organizationId;
    }
}
