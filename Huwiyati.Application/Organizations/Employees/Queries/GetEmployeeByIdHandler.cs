namespace Huwiyati.Application.Organizations.Employees.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Employees.DTOs;

public class GetEmployeeByIdHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public GetEmployeeByIdHandler(
        IApplicationDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<ApiResponse<EmployeeDto>> GetEmployeeByIdAsync(
        Guid currentAdminUserId,
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        // 1. Resolve Admin's BranchId from token UserId
        var adminBranchId = await _context.Employees
            .Where(e => e.UserId == currentAdminUserId && e.IsActive)
            .Select(e => (Guid?)e.BranchId)
            .FirstOrDefaultAsync(cancellationToken);

        if (!adminBranchId.HasValue)
        {
            return ApiResponse<EmployeeDto>.Failure("Current logged-in admin is not associated with an active branch.", statusCode: 403);
        }

        // 2. Find employee record ensuring it belongs to current admin's branch
        var empData = await _context.Employees
            .AsNoTracking()
            .Where(e => e.Id == employeeId && e.BranchId == adminBranchId.Value)
            .Select(e => new
            {
                Employee = e,
                BranchName = e.Branch.BranchName,
                OrganizationName = e.Branch.Organization.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (empData == null)
        {
            return ApiResponse<EmployeeDto>.Failure("Employee was not found in your branch.", statusCode: 404);
        }

        // 3. Resolve Person & Identity contact details
        var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(empData.Employee.UserId, cancellationToken);
        
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

        var dto = new EmployeeDto
        {
            EmployeeId = empData.Employee.Id,
            UserId = empData.Employee.UserId,
            NationalNumber = nationalNumber,
            FullName = fullName,
            Email = contactInfo?.Email ?? string.Empty,
            PhoneNumber = contactInfo?.PhoneNumber ?? string.Empty,
            EmployeeNumber = empData.Employee.EmployeeNumber,
            BranchId = empData.Employee.BranchId,
            BranchName = empData.BranchName,
            OrganizationName = empData.OrganizationName,
            IsActive = empData.Employee.IsActive,
            CreatedAt = empData.Employee.CreatedAt
        };

        return ApiResponse<EmployeeDto>.Success(dto);
    }
}
