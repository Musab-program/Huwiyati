namespace Huwiyati.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common.Interfaces;

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

    public Task<string> GenerateBirthCertificateNumberAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return GenerateDocumentNumberInternalAsync("03", branchId, cancellationToken);
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

        // 3. Sequential Number calculation with do-while verification against existing DB records
        int countOffset = 0;
        string candidateNumber;
        bool exists;

        do
        {
            int baseCount;
            if (serviceCode == "03")
            {
                baseCount = await _context.BirthCertificates.CountAsync(cancellationToken);
            }
            else if (serviceCode == "02")
            {
                baseCount = await _context.Families.Select(f => f.FamilyNumber).Distinct().CountAsync(cancellationToken);
            }
            else
            {
                baseCount = await _context.Persons.CountAsync(cancellationToken);
            }

            var sequenceNumber = (baseCount + 1 + countOffset).ToString("D6");
            candidateNumber = $"{serviceCode}{branchCodeStr}{sequenceNumber}";

            if (serviceCode == "03")
            {
                exists = await _context.BirthCertificates.AnyAsync(b => b.CertificateNumber == candidateNumber, cancellationToken);
            }
            else if (serviceCode == "02")
            {
                exists = await _context.Families.AnyAsync(f => f.FamilyNumber == candidateNumber, cancellationToken);
            }
            else
            {
                exists = await _context.Persons.AnyAsync(p => p.NationalNumber == candidateNumber, cancellationToken);
            }

            if (exists)
            {
                countOffset++;
            }
        } while (exists);

        return candidateNumber;
    }
}
