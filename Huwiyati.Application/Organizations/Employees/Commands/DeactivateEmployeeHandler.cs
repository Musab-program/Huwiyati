namespace Huwiyati.Application.Organizations.Employees.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

public class DeactivateEmployeeHandler
{
    private readonly IApplicationDbContext _context;

    public DeactivateEmployeeHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> DeactivateEmployeeAsync(
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
            return ApiResponse<bool>.Failure("Current logged-in admin is not associated with an active branch.", statusCode: 403);
        }

        // 2. Get employee from database ensuring branch scoping
        var emp = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeId && e.BranchId == adminBranchId.Value, cancellationToken);

        if (emp == null)
        {
            return ApiResponse<bool>.Failure("Employee was not found in your branch.", statusCode: 404);
        }

        if (!emp.IsActive)
        {
            return ApiResponse<bool>.Failure("Employee is already deactivated.", statusCode: 400);
        }

        // 3. Update IsActive status to false
        emp.IsActive = false;
        _context.Employees.Update(emp);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Success(true, message: "Employee deactivated successfully.");
    }
}
