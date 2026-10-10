namespace Huwiyati.Application.Documents.DeathCertificate.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.DeathCertificate.DTOs;

public class GetCitizenFamilyDeathCertificatesHandler
{
    private readonly IApplicationDbContext _context;

    public GetCitizenFamilyDeathCertificatesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<DeathCertificateDto>>> GetFamilyDeathCertificatesAsync(
        Guid citizenPersonId,
        CancellationToken cancellationToken = default)
    {
        if (citizenPersonId == Guid.Empty)
        {
            return ApiResponse<List<DeathCertificateDto>>.Failure("Invalid citizen identity.", statusCode: 400);
        }

        // 1. Find all family member Person IDs in families linked to this citizen
        var deceasedPersonIds = await _context.Families
            .AsNoTracking()
            .Where(f => f.HeadOfFamilyPersonId == citizenPersonId || f.FamilyMembers.Any(m => m.PersonId == citizenPersonId))
            .SelectMany(f => f.FamilyMembers.Select(m => m.PersonId).Concat(new[] { f.HeadOfFamilyPersonId }))
            .Distinct()
            .ToListAsync(cancellationToken);

        // 2. Query Death Certificates for any deceased person IDs found in the family
        var deathCertificates = await _context.DeathCertificates
            .AsNoTracking()
            .Where(d => deceasedPersonIds.Contains(d.PersonId))
            .Select(d => new DeathCertificateDto
            {
                Id = d.Id,
                CertificateNumber = d.CertificateNumber,
                IssueDate = d.IssueDate,
                QrCodePayload = d.QrCodePayload,
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

        return ApiResponse<List<DeathCertificateDto>>.Success(
            deathCertificates, message: "Deceased family member death certificates retrieved successfully.", statusCode: 200);
    }
}
