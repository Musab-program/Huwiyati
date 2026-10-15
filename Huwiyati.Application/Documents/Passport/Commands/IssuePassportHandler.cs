namespace Huwiyati.Application.Documents.Passport.Commands;

using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Extensions;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;

using Huwiyati.Domain.Events.Passport;

/// <summary>
/// Handles business logic for issuing a passport to an eligible citizen with 6-year validity.
/// </summary>
public class IssuePassportHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IDocumentNumberGenerator _documentNumberGenerator;
    private readonly IQRCodeService _qrCodeService;
    private readonly IDomainEventHandler<PassportIssuedEvent> _passportIssuedEventHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="IssuePassportHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="documentNumberGenerator">The generator for official passport numbers.</param>
    /// <param name="qrCodeService">The QR code payload generation service.</param>
    /// <param name="passportIssuedEventHandler">The domain event handler for passport issuance notifications.</param>
    public IssuePassportHandler(
        IApplicationDbContext context,
        IDocumentNumberGenerator documentNumberGenerator,
        IQRCodeService qrCodeService,
        IDomainEventHandler<PassportIssuedEvent> passportIssuedEventHandler)
    {
        _context = context;
        _documentNumberGenerator = documentNumberGenerator;
        _qrCodeService = qrCodeService;
        _passportIssuedEventHandler = passportIssuedEventHandler;
    }

    /// <summary>
    /// Validates immigration branch, verifies citizen identity and active passport absence, generates number, and creates the passport.
    /// </summary>
    /// <param name="command">The command carrying citizen identifier, branch, and passport type.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the issued <see cref="PassportDto"/>.</returns>
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
            QrCodePayload = _qrCodeService.GenerateVerificationToken(),
            Status = PassportStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Passports.AddAsync(passport, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 8. Trigger Domain Event to create notification asynchronously/decoupled
        var passportIssuedEvent = new PassportIssuedEvent(person.Id, passport.PassportNumber, passport.ExpiryDate);
        await _passportIssuedEventHandler.HandleAsync(
           passportIssuedEvent, cancellationToken);

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
