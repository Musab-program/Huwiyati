namespace Huwiyati.Application.Documents.NationalIdCard.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

/// <summary>
/// Handles querying all active National ID Cards across the civil registry.
/// </summary>
public class GetNationalIdCardsHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetNationalIdCardsHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetNationalIdCardsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all active National ID Cards ordered chronologically by creation timestamp.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of <see cref="NationalIdCardDto"/>.</returns>
    public async Task<ApiResponse<List<NationalIdCardDto>>> GetActiveNationalIdCardsAsync(
        CancellationToken cancellationToken = default)
    {
        var activeCards = await _context.NationalIdCards
            .AsNoTracking()
            .Where(c => c.Status == NationalIdCardStatus.Active)
            .OrderByDescending(c => c.CreatedAt)
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

        return ApiResponse<List<NationalIdCardDto>>.Success(activeCards);
    }
}
