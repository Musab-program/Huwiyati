namespace Huwiyati.Application.Documents.DeathCertificate.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.DeathCertificate.DTOs;

public class UpdateDeathCertificateHandler
{
    private readonly IApplicationDbContext _context;

    public UpdateDeathCertificateHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<DeathCertificateDto>> UpdateAsync(
        UpdateDeathCertificateCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Fetch DeathCertificate entity by ID
        var deathCertificate = await _context.DeathCertificates
            .FirstOrDefaultAsync(d => d.Id == command.DeathCertificateId, cancellationToken);

        if (deathCertificate == null)
        {
            return ApiResponse<DeathCertificateDto>.Failure("Specified death certificate record was not found.", statusCode: 404);
        }

        // 2. Update DeathCertificate properties
        deathCertificate.DeathDate = command.DeathDate;
        deathCertificate.PlaceOfDeath = command.PlaceOfDeath;
        deathCertificate.CauseOfDeath = command.CauseOfDeath;

        // 3. Update associated FamilyMember LeftAt date for deceased member
        var familyMembers = await _context.FamilyMembers
            .Where(m => m.PersonId == deathCertificate.PersonId && m.Status == FamilyMemberStatus.Deceased)
            .ToListAsync(cancellationToken);

        foreach (var member in familyMembers)
        {
            member.LeftAt = command.DeathDate.ToDateTime(TimeOnly.MinValue);
        }

        // 4. Save changes to database
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Query updated DeathCertificateDto projection cleanly via Select
        var responseDto = await _context.DeathCertificates
            .AsNoTracking()
            .Where(d => d.Id == deathCertificate.Id)
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
            .FirstOrDefaultAsync(cancellationToken);

        return ApiResponse<DeathCertificateDto>.Success(responseDto!, message: "Death certificate details updated successfully.");
    }
}
