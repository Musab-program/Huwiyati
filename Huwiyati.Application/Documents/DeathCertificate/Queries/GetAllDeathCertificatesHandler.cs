namespace Huwiyati.Application.Documents.DeathCertificate.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.DeathCertificate.DTOs;

/// <summary>
/// Handles querying all issued death certificates across the civil registry.
/// </summary>
public class GetAllDeathCertificatesHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllDeathCertificatesHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetAllDeathCertificatesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all recorded death certificates ordered descending by creation date.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of <see cref="DeathCertificateDto"/>.</returns>
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

        return ApiResponse<List<DeathCertificateDto>>.Success(certificates);
    }
}
