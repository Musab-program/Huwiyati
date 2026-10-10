namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;
using Huwiyati.Domain.Enums;

public class GetActivePassportByPersonIdHandler
{
    private readonly IApplicationDbContext _context;

    public GetActivePassportByPersonIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

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
