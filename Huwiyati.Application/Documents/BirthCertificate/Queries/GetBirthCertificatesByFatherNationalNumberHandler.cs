namespace Huwiyati.Application.Documents.BirthCertificate.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.BirthCertificate.DTOs;

using Huwiyati.Domain.Enums;

/// <summary>
/// Handles querying all birth certificates associated with a father or mother identified by national number.
/// </summary>
public class GetBirthCertificatesByFatherNationalNumberHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetBirthCertificatesByFatherNationalNumberHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetBirthCertificatesByFatherNationalNumberHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all birth certificates where the person acts as either the father or the mother.
    /// </summary>
    /// <param name="fatherNationalNumber">The national number of the parent.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the list of matching <see cref="BirthCertificateDto"/>.</returns>
    public async Task<ApiResponse<List<BirthCertificateDto>>> GetByFatherNationalNumberAsync(
        string fatherNationalNumber,
        CancellationToken cancellationToken = default)
    {
        var parent = await _context.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.NationalNumber == fatherNationalNumber, cancellationToken);

        if (parent == null)
        {
            return ApiResponse<List<BirthCertificateDto>>.Failure("No person record found with the provided national number.", statusCode: 404);
        }

        var certificates = await _context.BirthCertificates
            .AsNoTracking()
            .Where(b => b.FatherPerson.NationalNumber == fatherNationalNumber || b.MotherPerson.NationalNumber == fatherNationalNumber)
            .OrderByDescending(b => b.IssueDate)
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
            .ToListAsync(cancellationToken);

        return ApiResponse<List<BirthCertificateDto>>.Success(certificates);
    }
}
