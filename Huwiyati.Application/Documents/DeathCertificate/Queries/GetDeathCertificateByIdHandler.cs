namespace Huwiyati.Application.Documents.DeathCertificate.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.DeathCertificate.DTOs;

public class GetDeathCertificateByIdHandler
{
    private readonly IApplicationDbContext _context;

    public GetDeathCertificateByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<DeathCertificateDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var certificateDto = await _context.DeathCertificates
            .AsNoTracking()
            .Where(d => d.Id == id)
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

        if (certificateDto == null)
        {
            return ApiResponse<DeathCertificateDto>.Failure("Death certificate with the specified ID was not found.", statusCode: 404);
        }

        return ApiResponse<DeathCertificateDto>.Success(certificateDto);
    }
}
