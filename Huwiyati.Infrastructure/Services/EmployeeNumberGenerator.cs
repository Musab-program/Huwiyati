namespace Huwiyati.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Generator service creating standardized employee numbers formatted as OrgCode-Year-BranchIndex-Sequence.
/// </summary>
public class EmployeeNumberGenerator : IEmployeeNumberGenerator
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeNumberGenerator"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public EmployeeNumberGenerator(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Generates a standardized employee number for a given branch asynchronously.
    /// </summary>
    /// <param name="branchId">The branch identifier to generate the number for.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A formatted employee number string.</returns>
    public async Task<string> GenerateEmployeeNumberAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        // 1. Fetch branch details and its parent organization code/name via projection
        var branch = await _context.OrganizationBranches
            .Where(b => b.Id == branchId)
            .Select(b => new
            {
                b.Id,
                b.OrganizationId,
                OrganizationName = b.Organization.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (branch == null) return $"EMP-{DateTime.UtcNow.Year}-0001";

        // 2. Map organization to 2-digit code
        string orgCode = branch.OrganizationName switch
        {
            "الأحوال المدنية" => "01",
            "الجوازات والهجرة" => "02",
            "المرور" => "03",
            "المستشفيات" => "04",
            _ => "09"
        };

        var currentYear = DateTime.UtcNow.Year;

        // 3. Get exact 1-based index position of THIS specific branch within its organization
        var organizationBranchIds = await _context.OrganizationBranches
            .Where(b => b.OrganizationId == branch.OrganizationId)
            .OrderBy(b => b.CreatedAt)
            .Select(b => b.Id)
            .ToListAsync(cancellationToken);

        int branchIndex = organizationBranchIds.IndexOf(branch.Id) + 1; // e.g. 1, 10, 15...

        // 4. Count total employees in THIS specific branch to get next sequence
        int employeeIndex = await _context.Employees
            .CountAsync(e => e.BranchId == branchId, cancellationToken) + 1;

        // Format Result: 01-2026-015-0001
        return $"{orgCode}-{currentYear}-{branchIndex:D3}-{employeeIndex:D4}";
    }
}