namespace Huwiyati.Application.Documents.Passport.Commands;

using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Extensions;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;

// Handler carrying out the business logic for issuing a Passport
public class IssuePassportHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IDocumentNumberGenerator _documentNumberGenerator;

    public IssuePassportHandler(
        IApplicationDbContext context,
        IDocumentNumberGenerator documentNumberGenerator)
    {
        _context = context;
        _documentNumberGenerator = documentNumberGenerator;
    }

    public async Task<ApiResponse<PassportDto>> IssueAsync(
        IssuePassportCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Validate Immigration & Passports Issuing Branch
        var branchResult = await _context.ValidateImmigrationBranchAsync(command.IssuingBranchId, cancellationToken);
        if (!branchResult.IsValid)
        {
            return ApiResponse<PassportDto>.Failure(branchResult.ErrorMessage, statusCode: branchResult.StatusCode);
        }

        // 2. Validate Person existence
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<PassportDto>.Failure(
                "Person with the provided ID was not found.", statusCode: 404);
        }

        // 3. Ensure Person does not already have an Active Passport
        var activePassportExists = await _context.Passports
            .AnyAsync(p => p.PersonId == person.Id && p.Status == PassportStatus.Active, cancellationToken);

        if (activePassportExists)
        {
            return ApiResponse<PassportDto>.Failure(
                "This person already possesses an active Passport.", statusCode: 400);
        }

        // 4. Update PhotoUrl if provided in request
        if (!string.IsNullOrWhiteSpace(command.PhotoUrl))
        {
            person.PhotoUrl = command.PhotoUrl;
        }

        // 5. Calculate IssueDate (Today) and ExpiryDate (fixed +6 years)
        var issueDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var expiryDate = issueDate.AddYears(6);

        // 6. Generate Passport Number with service code "05"
        var passportNumber = await _documentNumberGenerator.GeneratePassportNumberAsync(command.IssuingBranchId, cancellationToken);

        // 7. Instantiate and add Passport entity
        var passport = new Passport
        {
            PersonId = person.Id,
            IssuingBranchId = command.IssuingBranchId,
            PassportNumber = passportNumber,
            PassportType = command.PassportType,
            IssueDate = issueDate,
            ExpiryDate = expiryDate,
            QrCodePayload = $"PASS-{passportNumber}",
            Status = PassportStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Passports.AddAsync(passport, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 8. Map response DTO
        var responseDto = new PassportDto
        {
            Id = passport.Id,
            PersonId = person.Id,
            PersonFullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim(),
            NationalNumber = person.NationalNumber,
            PhotoUrl = person.PhotoUrl,
            PassportNumber = passport.PassportNumber,
            PassportType = passport.PassportType,
            IssueDate = passport.IssueDate,
            ExpiryDate = passport.ExpiryDate,
            QrCodePayload = passport.QrCodePayload,
            Status = passport.Status,
            IssuingBranchId = branchResult.BranchId,
            IssuingBranchName = branchResult.BranchName,
            CreatedAt = passport.CreatedAt
        };

        return ApiResponse<PassportDto>.Success(
            responseDto, message: "Passport issued successfully.", statusCode: 201);
    }
}
