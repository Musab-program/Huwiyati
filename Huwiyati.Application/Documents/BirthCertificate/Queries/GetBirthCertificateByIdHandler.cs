namespace Huwiyati.Application.Documents.BirthCertificate.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.BirthCertificate.DTOs;

public class GetBirthCertificateByIdHandler
{
    private readonly IApplicationDbContext _context;

    public GetBirthCertificateByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<BirthCertificateDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var certificateDto = await _context.BirthCertificates
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BirthCertificateDto
            {
                Id = b.Id,
                CertificateNumber = b.CertificateNumber,
                IssueDate = b.IssueDate,

                ChildPersonId = b.ChildPersonId,
                ChildNationalNumber = b.ChildPerson.NationalNumber,
                ChildFullName = $"{b.ChildPerson.FirstName} {b.ChildPerson.FatherName} {b.ChildPerson.GrandfatherName} {b.ChildPerson.FamilyName}".Trim(),
                Gender = b.ChildPerson.Gender.ToString(),
                DateOfBirth = b.ChildPerson.DateOfBirth,
                PlaceOfBirth = b.ChildPerson.PlaceOfBirth,

                FatherPersonId = b.FatherPersonId,
                FatherNationalNumber = b.FatherPerson.NationalNumber,
                FatherFullName = $"{b.FatherPerson.FirstName} {b.FatherPerson.FatherName} {b.FatherPerson.GrandfatherName} {b.FatherPerson.FamilyName}".Trim(),

                MotherPersonId = b.MotherPersonId,
                MotherNationalNumber = b.MotherPerson.NationalNumber,
                MotherFullName = $"{b.MotherPerson.FirstName} {b.MotherPerson.FatherName} {b.MotherPerson.GrandfatherName} {b.MotherPerson.FamilyName}".Trim(),

                HospitalBranchId = b.HospitalBranchId,
                HospitalName = b.HospitalBranch.BranchName,
                CreatedAt = b.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (certificateDto == null)
        {
            return ApiResponse<BirthCertificateDto>.Failure("Birth certificate with the specified ID was not found.", statusCode: 404);
        }

        return ApiResponse<BirthCertificateDto>.Success(certificateDto);
    }
}
