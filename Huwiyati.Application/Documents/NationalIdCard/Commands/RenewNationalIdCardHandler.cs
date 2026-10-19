namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;
using Huwiyati.Application.Common.Extensions;

using Huwiyati.Domain.Events.Documents;

/// <summary>
/// Handles renewing an existing National ID Card under 90-day expiry eligibility rules.
/// </summary>
public class RenewNationalIdCardHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IQRCodeService _qrCodeService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDomainEventHandler<NationalIdCardRenewedEvent> _nationalIdCardRenewedEventHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="RenewNationalIdCardHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="qrCodeService">The QR code payload generation service.</param>
    /// <param name="fileStorageService">The file storage service for photo management.</param>
    /// <param name="nationalIdCardRenewedEventHandler">The domain event handler for card renewal notifications.</param>
    public RenewNationalIdCardHandler(
        IApplicationDbContext context,
        IQRCodeService qrCodeService,
        IFileStorageService fileStorageService,
        IDomainEventHandler<NationalIdCardRenewedEvent> nationalIdCardRenewedEventHandler)
    {
        _context = context;
        _qrCodeService = qrCodeService;
        _fileStorageService = fileStorageService;
        _nationalIdCardRenewedEventHandler = nationalIdCardRenewedEventHandler;
    }

    /// <summary>
    /// Verifies 90-day renewal eligibility, archives previous active card, updates profile data, and creates a new 10-year card.
    /// </summary>
    /// <param name="command">The command carrying citizen identifier and updated card parameters.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the renewed <see cref="NationalIdCardDto"/>.</returns>
    public async Task<ApiResponse<NationalIdCardDto>> RenewAsync(
        RenewNationalIdCardCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Search for Person by National Number
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                "No citizen record found with the provided National Number.", statusCode: 404);
        }

        if (person.PersonStatus == PersonStatus.Deceased)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                "Cannot renew a national ID card for a deceased citizen.", statusCode: 400);
        }

        // 2. Verify issuing branch exists
        var branch = await _context.ValidateCivilRegistryBranchAsync(command.IssuingBranchId, cancellationToken);
        if (!branch.IsValid)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                branch.ErrorMessage,
                statusCode: branch.StatusCode
                );
        }
        
        // 3. Find existing Active National ID Card for this Person
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var activeCard = await _context.NationalIdCards
            .FirstOrDefaultAsync(c => c.PersonId == person.Id && c.Status == NationalIdCardStatus.Active, cancellationToken);

        // If an Active card exists (meaning it hasn't turned Expired automatically yet), check remaining days
        if (activeCard != null)
        {
            var daysRemaining = activeCard.ExpiryDate.DayNumber - today.DayNumber;

            // If remaining valid days is strictly more than 3 months (90 days), disallow early renewal
            if (daysRemaining > 90)
            {
                return ApiResponse<NationalIdCardDto>.Failure(
                    $"Card cannot be renewed yet. Renewal is only allowed within 3 months (90 days) prior to expiry date. Remaining valid days: {daysRemaining}.",
                    statusCode: 400);
            }

            // Remaining days <= 90 -> Mark the active card as Expired
            activeCard.Status = NationalIdCardStatus.Expired;
        }

        // 4. Update Person optional profile fields cleanly
        await UpdatePersonProfileIfProvidedAsync(person, command, cancellationToken);

        // 5. Calculate new IssueDate (today UTC) and ExpiryDate (+10 years)
        var newIssueDate = today;
        var newExpiryDate = newIssueDate.AddYears(10);

        // 6. Create NEW NationalIdCard entity with a new Guid Version 7
        var newCard = new NationalIdCard
        {
            PersonId = person.Id,
            IssuingBranchId = command.IssuingBranchId,
            IssueDate = newIssueDate,
            ExpiryDate = newExpiryDate,
            QrCodePayload = _qrCodeService.GenerateVerificationToken(),
            Status = NationalIdCardStatus.Active
        };

        await _context.NationalIdCards.AddAsync(newCard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 6.1 Trigger Domain Event to notify citizen
        var renewEvent = new NationalIdCardRenewedEvent(person.Id, person.NationalNumber);
        await _nationalIdCardRenewedEventHandler.HandleAsync(renewEvent, cancellationToken);

        // 7. Map to DTO and return response
        var responseDto = new NationalIdCardDto
        {
            Id = newCard.Id,
            PersonId = person.Id,
            NationalNumber = person.NationalNumber,
            FullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim(),
            PhotoUrl = person.PhotoUrl,
            IssuingBranchId = branch.BranchId,
            BranchName = branch.BranchName,
            IssueDate = newCard.IssueDate,
            ExpiryDate = newCard.ExpiryDate,
            QrCodePayload = newCard.QrCodePayload,
            Status = newCard.Status.ToString(),
            CreatedAt = newCard.CreatedAt
        };

        return ApiResponse<NationalIdCardDto>.Success(
            responseDto, message: "National ID Card renewed successfully with a new 10-year validity.", statusCode: 200);
    }

    // Encapsulated method updating optional Person profile fields and cleaning up old photo file
    private async Task UpdatePersonProfileIfProvidedAsync(Person person, RenewNationalIdCardCommand command, CancellationToken ct)
    {
        if (command.MaritalStatus.HasValue) person.MaritalStatus = command.MaritalStatus.Value;
        if (!string.IsNullOrWhiteSpace(command.Governorate)) person.Governorate = command.Governorate;
        if (!string.IsNullOrWhiteSpace(command.District)) person.District = command.District;
        if (!string.IsNullOrWhiteSpace(command.AddressDetails)) person.AddressDetails = command.AddressDetails;
        
        if (!string.IsNullOrWhiteSpace(command.PhotoUrl))
        {
            if (!string.IsNullOrWhiteSpace(person.PhotoUrl) && person.PhotoUrl != command.PhotoUrl)
            {
                await _fileStorageService.DeletePhotoAsync(person.PhotoUrl, ct);
            }
            person.PhotoUrl = command.PhotoUrl;
        }
    }
}
