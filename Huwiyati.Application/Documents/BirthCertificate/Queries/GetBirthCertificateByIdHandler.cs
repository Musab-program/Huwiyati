namespace Huwiyati.Application.Documents.BirthCertificate.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.BirthCertificate.DTOs;

/// <summary>
/// Handles querying a specific birth certificate by its unique database identifier.
/// </summary>
public class GetBirthCertificateByIdHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetBirthCertificateByIdHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetBirthCertificateByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a birth certificate projected into <see cref="BirthCertificateDto"/> by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the birth certificate.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the matching <see cref="BirthCertificateDto"/> or a 404 response.</returns>
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
                QrCodePayload = b.QrCodePayload,

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
