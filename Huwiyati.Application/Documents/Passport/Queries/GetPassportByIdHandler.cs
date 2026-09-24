namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;
using Huwiyati.Domain.Enums;

// Handler returning single Active Passport details by Passport ID
public class GetPassportByIdHandler
{
    private readonly IApplicationDbContext _context;

    public GetPassportByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

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
