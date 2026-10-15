namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;
using Huwiyati.Domain.Enums;

/// <summary>
/// Handles querying all active passports registered in the system.
/// </summary>
public class GetActivePassportsHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetActivePassportsHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetActivePassportsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all active passports ordered chronologically by creation timestamp.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of <see cref="PassportDto"/>.</returns>
    public async Task<ApiResponse<List<PassportDto>>> GetActivePassportsAsync(
        CancellationToken cancellationToken = default)
    {
        var activePassports = await _context.Passports
            .AsNoTracking()
            .Where(p => p.Status == PassportStatus.Active)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PassportDto
            {
                Id = p.Id,
                PersonId = p.PersonId,
                PersonFullName = $"{p.Person.FirstName} {p.Person.FatherName} {p.Person.GrandfatherName} {p.Person.FamilyName}".Trim(),
                NationalNumber = p.Person.NationalNumber,
                PhotoUrl = p.Person.PhotoUrl,
                PassportNumber = p.PassportNumber,
                PassportType = p.PassportType,
                IssueDate = p.IssueDate,
                ExpiryDate = p.ExpiryDate,
                QrCodePayload = p.QrCodePayload,
                Status = p.Status,
                IssuingBranchId = p.IssuingBranchId,
                IssuingBranchName = p.IssuingBranch.BranchName,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<PassportDto>>.Success(activePassports);
    }
}
