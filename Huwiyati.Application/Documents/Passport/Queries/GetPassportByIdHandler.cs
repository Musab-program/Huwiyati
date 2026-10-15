namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;
using Huwiyati.Domain.Enums;

/// <summary>
/// Handles querying details of an active passport by its document identifier.
/// </summary>
public class GetPassportByIdHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPassportByIdHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetPassportByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves active passport details projected into <see cref="PassportDto"/> by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the passport record.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the matching <see cref="PassportDto"/> or a 404 response.</returns>
    public async Task<ApiResponse<PassportDto>> GetPassportByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var passportDto = await _context.Passports
            .AsNoTracking()
            .Where(p => p.Id == id && p.Status == PassportStatus.Active)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (passportDto == null)
        {
            return ApiResponse<PassportDto>.Failure(
                "Active Passport with the specified ID was not found.",
                statusCode: 404);
        }

        return ApiResponse<PassportDto>.Success(passportDto);
    }
}
