namespace Huwiyati.Application.Organizations.Employees.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

using Huwiyati.Domain.Events.Organizations;

public class ActivateEmployeeHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IDomainEventHandler<EmployeeActivatedEvent> _employeeActivatedEventHandler;

    public ActivateEmployeeHandler(
        IApplicationDbContext context,
        IDomainEventHandler<EmployeeActivatedEvent> employeeActivatedEventHandler)
    {
        _context = context;
        _employeeActivatedEventHandler = employeeActivatedEventHandler;
    }

    public async Task<ApiResponse<bool>> ActivateEmployeeAsync(
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

        if (emp.IsActive)
        {
            return ApiResponse<bool>.Failure("Employee is already active.", statusCode: 400);
        }

        // 3. Update IsActive status to true
        emp.IsActive = true;
        _context.Employees.Update(emp);
        await _context.SaveChangesAsync(cancellationToken);

        // 3.1 Trigger Domain Event to notify employee user account
        var branchName = await _context.OrganizationBranches
            .Where(b => b.Id == emp.BranchId)
            .Select(b => b.BranchName)
            .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

        var activateEvent = new EmployeeActivatedEvent(emp.UserId, branchName);
        await _employeeActivatedEventHandler.HandleAsync(activateEvent, cancellationToken);

        return ApiResponse<bool>.Success(true, message: "Employee activated successfully.");
    }
}
