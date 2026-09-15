namespace Huwiyati.Application.Organizations.Employees.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Employees.DTOs;

public class GetEmployeesHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public GetEmployeesHandler(
        IApplicationDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<ApiResponse<List<EmployeeDto>>> GetEmployeesAsync(
        Guid currentAdminUserId,
        CancellationToken cancellationToken = default)
    {
        // 1. Resolve Admin's BranchId from token UserId
        var adminBranchId = await _context.Employees
            .Where(e => e.UserId == currentAdminUserId && e.IsActive)
            .Select(e => (Guid?)e.BranchId)
            .FirstOrDefaultAsync(cancellationToken);

        if (!adminBranchId.HasValue)
        {
            return ApiResponse<List<EmployeeDto>>.Failure("Current logged-in admin is not associated with an active branch.", statusCode: 403);
        }

        // 2. Fetch employees belonging strictly to admin's branch without Include
        var rawEmployees = await _context.Employees
            .AsNoTracking()
            .Where(e => e.BranchId == adminBranchId.Value)
            .Select(e => new
            {
                Employee = e,
                BranchName = e.Branch.BranchName,
                OrganizationName = e.Branch.Organization.Name
            })
            .ToListAsync(cancellationToken);

        var resultList = new List<EmployeeDto>();

        // 3. Construct DTO list with Person details via IdentityService
        foreach (var item in rawEmployees)
        {
            var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(item.Employee.UserId, cancellationToken);
            
            string nationalNumber = string.Empty;
            string fullName = string.Empty;

            if (contactInfo.HasValue && contactInfo.Value.PersonId != Guid.Empty)
            {
                var person = await _context.Persons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == contactInfo.Value.PersonId, cancellationToken);

                if (person != null)
                {
                    nationalNumber = person.NationalNumber;
                    fullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}";
                }
            }

            resultList.Add(new EmployeeDto
            {
                EmployeeId = item.Employee.Id,
                UserId = item.Employee.UserId,
                NationalNumber = nationalNumber,
                FullName = fullName,
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

        return ApiResponse<List<EmployeeDto>>.Success(resultList);
    }
}
