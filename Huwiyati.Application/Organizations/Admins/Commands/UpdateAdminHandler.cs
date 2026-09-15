namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Admins.DTOs;
using Huwiyati.Domain.Constants;

public class UpdateAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public UpdateAdminHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<ApiResponse<AdminDto>> UpdateAdminAsync(
        Guid employeeId,
        UpdateAdminCommand command,
        CancellationToken cancellationToken = default)
    {
        var emp = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);

        if (emp == null)
        {
            return ApiResponse<AdminDto>.Failure(
                "Admin record was not found.",
                statusCode: 404);
        }

        // Verify that this employee has the Admin role
        var roles = await _identityService.GetUserRolesAsync(emp.UserId, cancellationToken);
        if (!roles.Contains(AppRoles.Admin))
        {
            return ApiResponse<AdminDto>.Failure(
                "The specified employee is not an Admin.",
                statusCode: 400);
        }

        var newBranchData = await _context.OrganizationBranches
            .Where(b => b.Id == command.BranchId && b.IsActive)
            .Select(b => new
            {
                Branch = b,
                OrganizationName = b.Organization.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (newBranchData == null)
        {
            return ApiResponse<AdminDto>.Failure(
                "The target organization branch was not found or is inactive.",
                statusCode: 404);
        }

        emp.BranchId = command.BranchId;
        emp.LastModifiedAt = DateTime.UtcNow;

        _context.Employees.Update(emp);
        await _context.SaveChangesAsync(cancellationToken);

        var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(emp.UserId, cancellationToken);
        var person = contactInfo.HasValue
            ? await _context.Persons.FirstOrDefaultAsync(p => p.Id == contactInfo.Value.PersonId, cancellationToken)
            : null;

        var adminDto = new AdminDto
        {
            EmployeeId = emp.Id,
            UserId = emp.UserId,
            NationalNumber = person?.NationalNumber ?? string.Empty,
            FullName = person != null ? $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}" : string.Empty,
            Email = contactInfo?.Email ?? string.Empty,
            PhoneNumber = contactInfo?.PhoneNumber ?? string.Empty,
            EmployeeNumber = emp.EmployeeNumber,
            BranchId = emp.BranchId,
            BranchName = newBranchData.Branch.BranchName,
            OrganizationName = newBranchData.OrganizationName,
            IsActive = emp.IsActive,
            CreatedAt = emp.CreatedAt
        };

        return ApiResponse<AdminDto>.Success(adminDto, message: "Admin branch updated successfully.");
    }
}
