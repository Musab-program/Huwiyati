namespace Huwiyati.Application.Documents.NationalIdCard.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

/// <summary>
/// Handles querying details of an active National ID Card by its card identifier.
/// </summary>
public class GetNationalIdCardByIdHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetNationalIdCardByIdHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetNationalIdCardByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves active National ID Card information projected into <see cref="NationalIdCardDto"/> by card ID.
    /// </summary>
    /// <param name="id">The unique identifier of the national ID card.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the matching <see cref="NationalIdCardDto"/> or a 404 response.</returns>
    public async Task<ApiResponse<NationalIdCardDto>> GetNationalIdCardByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var cardDto = await _context.NationalIdCards
            .AsNoTracking()
            .Where(c => c.Id == id && c.Status == NationalIdCardStatus.Active)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (cardDto == null)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                "Active National ID Card with the specified ID was not found.",
                statusCode: 404);
        }

        return ApiResponse<NationalIdCardDto>.Success(cardDto);
    }
}
