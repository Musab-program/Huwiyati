namespace Huwiyati.Application.Organizations.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.DTOs;

public class GetOrganizationsQueryHandler
{
    private readonly IApplicationDbContext _context;

    public GetOrganizationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<OrganizationLookupDto>>> GetOrganizationsAsync(CancellationToken cancellationToken = default)
    {
        var organizations = await _context.Organizations
            .Where(o => o.IsActive)
            .Select(o => new OrganizationLookupDto
            {
                Id = o.Id,
                Name = o.Name
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<OrganizationLookupDto>>.Success(organizations);
    }
}