namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;
using Huwiyati.Domain.Enums;

// Handler returning list of active Passports using AsNoTracking and Select projection
public class GetActivePassportsHandler
{
    private readonly IApplicationDbContext _context;

    public GetActivePassportsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

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
