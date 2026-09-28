using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Requests.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.Application.Requests.Queries;

// Handler responsible for retrieving active ServiceTypes for drop-down menus
public class GetServiceTypesHandler
{
    private readonly IApplicationDbContext _context;

    public GetServiceTypesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ServiceTypeDto>>> GetServiceTypesAsync(
        GetServiceTypesQuery query,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.ServiceTypes
            .AsNoTracking()
            .Where(st => st.IsActive);

        if (query.OrganizationId.HasValue && query.OrganizationId.Value != Guid.Empty)
        {
            baseQuery = baseQuery.Where(st => st.OrganizationId == query.OrganizationId.Value);
        }

        var serviceTypes = await baseQuery
            .OrderBy(st => st.Name)
            .Select(st => new ServiceTypeDto
            {
                Id = st.Id,
                Name = st.Name,
                Code = st.Code,
                OrganizationId = st.OrganizationId,
                OrganizationName = st.Organization.Name,
                IsActive = st.IsActive
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<ServiceTypeDto>>.Success(serviceTypes, message: "Active service types retrieved successfully.", statusCode: 200);
    }
}
