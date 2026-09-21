namespace Huwiyati.Application.Documents.DeathCertificate.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.DeathCertificate.DTOs;

public class GetAllDeathCertificatesHandler
{
    private readonly IApplicationDbContext _context;

    public GetAllDeathCertificatesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<DeathCertificateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var certificates = await _context.DeathCertificates
            .AsNoTracking()
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new DeathCertificateDto
            {
                Id = d.Id,
                CertificateNumber = d.CertificateNumber,
                IssueDate = d.IssueDate,

                PersonId = d.PersonId,
                NationalNumber = d.Person.NationalNumber,
                FullName = $"{d.Person.FirstName} {d.Person.FatherName} {d.Person.GrandfatherName} {d.Person.FamilyName}".Trim(),
                DateOfBirth = d.Person.DateOfBirth,
                Gender = d.Person.Gender.ToString(),

                DeathDate = d.DeathDate,
                PlaceOfDeath = d.PlaceOfDeath,
                CauseOfDeath = d.CauseOfDeath,

                HospitalBranchId = d.HospitalBranchId,
                HospitalName = d.HospitalBranch.BranchName,

                IssuingBranchId = d.IssuingBranchId,
                IssuingBranchName = d.IssuingBranch.BranchName,

                CreatedAt = d.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<DeathCertificateDto>>.Success(certificates);
    }
}
