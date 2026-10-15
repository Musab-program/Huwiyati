namespace Huwiyati.Application.Documents.NationalIdCard.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

/// <summary>
/// Handles querying historical and current National ID Cards associated with a citizen.
/// </summary>
public class GetPersonNationalIdCardHistoryHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonNationalIdCardHistoryHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetPersonNationalIdCardHistoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all past and present cards for a given citizen identified by national number.
    /// </summary>
    /// <param name="nationalNumber">The 11-digit national number of the citizen.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of historical <see cref="NationalIdCardDto"/>.</returns>
    public async Task<ApiResponse<List<NationalIdCardDto>>> GetCardHistoryByNationalNumberAsync(
        string nationalNumber,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify citizen exists in Civil Registry
        var personExists = await _context.Persons
            .AsNoTracking()
            .AnyAsync(p => p.NationalNumber == nationalNumber, cancellationToken);

        if (!personExists)
        {
            return ApiResponse<List<NationalIdCardDto>>.Failure(
                "No citizen record found with the provided National Number in Civil Registry.",
                statusCode: 404);
        }

        // 2. Fetch full card history (Active, Expired, Lost, Suspended)
        var cardHistory = await _context.NationalIdCards
            .AsNoTracking()
            .Where(c => c.Person.NationalNumber == nationalNumber)
            .OrderByDescending(c => c.IssueDate)
            .Select(c => new NationalIdCardDto
            {
                Id = c.Id,
                PersonId = c.PersonId,
                NationalNumber = c.Person.NationalNumber,
                FullName = $"{c.Person.FirstName} {c.Person.FatherName} {c.Person.GrandfatherName} {c.Person.FamilyName}".Trim(),
                PhotoUrl = c.Person.PhotoUrl,
                IssuingBranchId = c.IssuingBranchId,
                BranchName = c.OrganizationBranch.BranchName,
                IssueDate = c.IssueDate,
                ExpiryDate = c.ExpiryDate,
                QrCodePayload = c.QrCodePayload,
                Status = c.Status.ToString(),
                CreatedAt = c.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<NationalIdCardDto>>.Success(cardHistory);
    }
}
