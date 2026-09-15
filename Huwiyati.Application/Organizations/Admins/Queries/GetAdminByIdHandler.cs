namespace Huwiyati.Application.Organizations.Admins.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Admins.DTOs;
using Huwiyati.Domain.Constants;

public class GetAdminByIdHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public GetAdminByIdHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<ApiResponse<AdminDto>> GetAdminByIdAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var empData = await _context.Employees
            .AsNoTracking()
            .Where(e => e.Id == employeeId)
            .Select(e => new
            {
                Employee = e,
                BranchName = e.Branch.BranchName,
                OrganizationName = e.Branch.Organization.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (empData == null)
        {
            return ApiResponse<AdminDto>.Failure(
                "Admin record was not found.",
                statusCode: 404);
        }

        // Verify that the employee is assigned the Admin role
        var roles = await _identityService.GetUserRolesAsync(empData.Employee.UserId, cancellationToken);
        if (!roles.Contains(AppRoles.Admin))
        {
            return ApiResponse<AdminDto>.Failure(
                "The specified employee is not an Admin.",
                statusCode: 404);
        }

        var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(empData.Employee.UserId, cancellationToken);
        var person = contactInfo.HasValue
            ? await _context.Persons.FirstOrDefaultAsync(p => p.Id == contactInfo.Value.PersonId, cancellationToken)
            : null;

        var adminDto = new AdminDto
        {
            EmployeeId = empData.Employee.Id,
            UserId = empData.Employee.UserId,
            NationalNumber = person?.NationalNumber ?? string.Empty,
            FullName = person != null ? $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}" : string.Empty,
            Email = contactInfo?.Email ?? string.Empty,
            PhoneNumber = contactInfo?.PhoneNumber ?? string.Empty,
            EmployeeNumber = empData.Employee.EmployeeNumber,
            BranchId = empData.Employee.BranchId,
            BranchName = empData.BranchName,
            OrganizationName = empData.OrganizationName,
            IsActive = empData.Employee.IsActive,
            CreatedAt = empData.Employee.CreatedAt
        };

        return ApiResponse<AdminDto>.Success(adminDto);
    }
}
