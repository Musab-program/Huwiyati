namespace Huwiyati.Application.Organizations.Admins.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Admins.DTOs;
using Huwiyati.Domain.Constants;

/// <summary>
/// Handles querying administrators filtered by organization or branch.
/// </summary>
public class GetAdminsHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAdminsHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity management service.</param>
    public GetAdminsHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Retrieves a list of administrators optionally filtered by organization and/or branch.
    /// </summary>
    /// <param name="organizationId">Optional organization filter identifier.</param>
    /// <param name="branchId">Optional branch filter identifier.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of <see cref="AdminDto"/> items.</returns>
    public async Task<ApiResponse<List<AdminDto>>> GetAdminsAsync(
        Guid? organizationId = null,
        Guid? branchId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Employees.AsNoTracking();

        if (branchId.HasValue)
        {
            query = query.Where(e => e.BranchId == branchId.Value);
        }
        else if (organizationId.HasValue)
        {
            query = query.Where(e => e.Branch.OrganizationId == organizationId.Value);
        }

        var employees = await query
            .Select(e => new
            {
                Employee = e,
                BranchName = e.Branch.BranchName,
                OrganizationName = e.Branch.Organization.Name
            })
            .ToListAsync(cancellationToken);

        var adminDtos = new List<AdminDto>();

        foreach (var item in employees)
        {
            // Verify user has Admin role
            var roles = await _identityService.GetUserRolesAsync(item.Employee.UserId, cancellationToken);
            if (!roles.Contains(AppRoles.Admin))
            {
                continue; // Exclude non-admin regular staff
            }

            var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(item.Employee.UserId, cancellationToken);
            var person = contactInfo.HasValue
                ? await _context.Persons.FirstOrDefaultAsync(p => p.Id == contactInfo.Value.PersonId, cancellationToken)
                : null;

            adminDtos.Add(new AdminDto
            {
                EmployeeId = item.Employee.Id,
                UserId = item.Employee.UserId,
                NationalNumber = person?.NationalNumber ?? string.Empty,
                FullName = person != null ? $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}" : string.Empty,
                Email = contactInfo?.Email ?? string.Empty,
                PhoneNumber = contactInfo?.PhoneNumber ?? string.Empty,
                EmployeeNumber = item.Employee.EmployeeNumber,
                BranchId = item.Employee.BranchId,
                BranchName = item.BranchName,
                OrganizationName = item.OrganizationName,
                IsActive = item.Employee.IsActive,
                CreatedAt = item.Employee.CreatedAt
            });
        }

        return ApiResponse<List<AdminDto>>.Success(adminDtos);
    }
}
