namespace Huwiyati.Application.Documents.Passport.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Extensions;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;

// Handler carrying out the business logic for renewing a Passport
public class RenewPassportHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IDocumentNumberGenerator _documentNumberGenerator;

    public RenewPassportHandler(
        IApplicationDbContext context,
        IDocumentNumberGenerator documentNumberGenerator)
    {
        _context = context;
        _documentNumberGenerator = documentNumberGenerator;
    }

    public async Task<ApiResponse<PassportDto>> RenewAsync(
        RenewPassportCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Validate Immigration & Passports Issuing Branch
        var branchResult = await _context.ValidateImmigrationBranchAsync(command.IssuingBranchId, cancellationToken);
        if (!branchResult.IsValid)
        {
            return ApiResponse<PassportDto>.Failure(branchResult.ErrorMessage, statusCode: branchResult.StatusCode);
        }

        // 2. Fetch Person by National Number
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<PassportDto>.Failure(
                "No person record found with the provided National Number.", statusCode: 404);
        }

        // 3. Find existing Active Passport
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var activePassport = await _context.Passports
            .FirstOrDefaultAsync(p => p.PersonId == person.Id && p.Status == PassportStatus.Active, cancellationToken);

        if (activePassport != null)
        {
            var daysRemaining = activePassport.ExpiryDate.DayNumber - today.DayNumber;

            // Disallow renewal if remaining valid days is strictly more than 6 months (180 days)
            if (daysRemaining > 180)
            {
                return ApiResponse<PassportDto>.Failure(
                    $"Passport cannot be renewed yet. Renewal is only permitted within 6 months (180 days) prior to expiry date. Remaining valid days: {daysRemaining}.",
                    statusCode: 400);
            }

            // Mark active passport as Expired upon issuing the new renewed passport
            activePassport.Status = PassportStatus.Expired;
        }

        // 4. Update photo if provided
        if (!string.IsNullOrWhiteSpace(command.PhotoUrl))
        {
            person.PhotoUrl = command.PhotoUrl;
        }

        // 5. Calculate new dates (Issue = Today, Expiry = Today + 6 years)
        var newIssueDate = today;
        var newExpiryDate = newIssueDate.AddYears(6);

        // 6. Generate NEW Passport Number with service code "05"
        var newPassportNumber = await _documentNumberGenerator.GeneratePassportNumberAsync(command.IssuingBranchId, cancellationToken);

        // 7. Instantiate NEW Passport Entity
        var newPassport = new Passport
        {
            PersonId = person.Id,
            IssuingBranchId = command.IssuingBranchId,
            PassportNumber = newPassportNumber,
            PassportType = command.PassportType ?? activePassport?.PassportType ?? PassportType.Regular,
            IssueDate = newIssueDate,
            ExpiryDate = newExpiryDate,
            QrCodePayload = $"PASS-{newPassportNumber}",
            Status = PassportStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Passports.AddAsync(newPassport, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 8. Map response DTO
        var responseDto = new PassportDto
        {
            Id = newPassport.Id,
            PersonId = person.Id,
            PersonFullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim(),
            NationalNumber = person.NationalNumber,
            PhotoUrl = person.PhotoUrl,
            PassportNumber = newPassport.PassportNumber,
            PassportType = newPassport.PassportType,
            IssueDate = newPassport.IssueDate,
            ExpiryDate = newPassport.ExpiryDate,
            QrCodePayload = newPassport.QrCodePayload,
            Status = newPassport.Status,
            IssuingBranchId = branchResult.BranchId,
            IssuingBranchName = branchResult.BranchName,
            CreatedAt = newPassport.CreatedAt
        };

        return ApiResponse<PassportDto>.Success(
            responseDto, message: "Passport renewed successfully with a new 6-year validity.", statusCode: 200);
    }
}
