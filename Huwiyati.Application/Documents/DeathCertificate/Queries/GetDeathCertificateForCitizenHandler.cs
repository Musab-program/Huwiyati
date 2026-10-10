namespace Huwiyati.Application.Documents.DeathCertificate.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.DeathCertificate.DTOs;

public class GetDeathCertificateForCitizenHandler
{
    private readonly IApplicationDbContext _context;

    public GetDeathCertificateForCitizenHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<DeathCertificateDto>> GetDeathCertificateAsync(
        Guid certificateId,
        Guid citizenPersonId,
        CancellationToken cancellationToken = default)
    {
        if (certificateId == Guid.Empty || citizenPersonId == Guid.Empty)
        {
            return ApiResponse<DeathCertificateDto>.Failure("Invalid request parameter or citizen identity.", statusCode: 400);
        }

        var certificate = await _context.DeathCertificates
            .AsNoTracking()
            .Where(d => d.Id == certificateId)
            .Select(d => new
            {
                Dto = new DeathCertificateDto
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
                },
                DeceasedPersonId = d.PersonId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (certificate == null)
        {
            return ApiResponse<DeathCertificateDto>.Failure("Death certificate record was not found.", statusCode: 404);
        }

        // Verify citizen family relationship to deceased person
        var isFamilyMember = await _context.Families
            .AsNoTracking()
            .AnyAsync(f =>
                (f.HeadOfFamilyPersonId == citizenPersonId || f.FamilyMembers.Any(m => m.PersonId == citizenPersonId)) &&
                (f.HeadOfFamilyPersonId == certificate.DeceasedPersonId || f.FamilyMembers.Any(m => m.PersonId == certificate.DeceasedPersonId)),
                cancellationToken);

        if (!isFamilyMember && certificate.DeceasedPersonId != citizenPersonId)
        {
            return ApiResponse<DeathCertificateDto>.Failure("Unauthorized. You can only view death certificates of immediate family members.", statusCode: 403);
        }

        return ApiResponse<DeathCertificateDto>.Success(certificate.Dto, message: "Death certificate retrieved successfully.", statusCode: 200);
    }
}
