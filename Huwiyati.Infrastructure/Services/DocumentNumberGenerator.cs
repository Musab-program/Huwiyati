namespace Huwiyati.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common.Interfaces;

// Service generating official document numbers (01 for National ID, 02 for Family Card) enforcing Civil Registry branch check
public class DocumentNumberGenerator : IDocumentNumberGenerator
{
    private readonly IApplicationDbContext _context;

    public DocumentNumberGenerator(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<string> GenerateNationalNumberAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return GenerateDocumentNumberInternalAsync("01", branchId, cancellationToken);
    }

    public Task<string> GenerateFamilyNumberAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return GenerateDocumentNumberInternalAsync("02", branchId, cancellationToken);
    }

    private async Task<string> GenerateDocumentNumberInternalAsync(string serviceCode, Guid branchId, CancellationToken cancellationToken)
    {
        // 1. Fetch branch data with projection via Select instead of Include for optimal performance
        var branchData = await _context.OrganizationBranches
            .Where(b => b.Id == branchId)
            .Select(b => new
            {
                b.Id,
                b.CreatedAt,
                b.IsActive,
                OrganizationIsActive = b.Organization.IsActive,
                OrganizationName = b.Organization.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (branchData == null || !branchData.IsActive || !branchData.OrganizationIsActive)
        {
            throw new InvalidOperationException("Specified organization branch was not found or is inactive.");
        }

        // Enforce Civil Registry organization validation rule
        if (!branchData.OrganizationName.Contains("الأحوال المدنية") && !branchData.OrganizationName.Contains("Civil Registry"))
        {
            throw new InvalidOperationException("Document numbers can only be issued by Civil Registry branches.");
        }

        // 2. Branch Ordinal Code (3 digits)
        var branchOrdinal = await _context.OrganizationBranches
            .CountAsync(b => b.CreatedAt <= branchData.CreatedAt, cancellationToken);
        string branchCodeStr = (branchOrdinal % 999).ToString("D3");

        // 3. Sequential Number calculation (6 digits)
        // For Family Number ("02"), count distinct existing families in database.
        // For National Number ("01"), count total persons registered in Civil Registry.
        int existingCount;

        if (serviceCode == "02")
        {
            existingCount = await _context.Families
                .Select(f => f.FamilyNumber)
                .Distinct()
                .CountAsync(cancellationToken);
        }
        else
        {
            existingCount = await _context.Persons.CountAsync(cancellationToken);
        }

        var sequenceNumber = (existingCount + 1).ToString("D6");

        // Combine into 11-digit document number (e.g. "01001000001" or "02001000001")
        return $"{serviceCode}{branchCodeStr}{sequenceNumber}";
    }
}
