namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;
using Huwiyati.Domain.Enums;

/// <summary>
/// Handles querying the currently active passport for a citizen identified by person ID.
/// </summary>
public class GetActivePassportByPersonIdHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetActivePassportByPersonIdHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetActivePassportByPersonIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves active passport details projected into <see cref="PassportDto"/> for a specified person.
    /// </summary>
    /// <param name="personId">The unique person identifier of the citizen.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the active <see cref="PassportDto"/> or a 404 response.</returns>
    public async Task<ApiResponse<PassportDto>> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default)
    {
        if (personId == Guid.Empty)
        {
            return ApiResponse<PassportDto>.Failure("Invalid citizen identity.", statusCode: 400);
        }

        var passportDto = await _context.Passports
            .AsNoTracking()
            .Where(p => p.PersonId == personId && p.Status == PassportStatus.Active)
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
                "Active Passport for this citizen was not found.",
                statusCode: 404);
        }

        return ApiResponse<PassportDto>.Success(passportDto);
    }
}
