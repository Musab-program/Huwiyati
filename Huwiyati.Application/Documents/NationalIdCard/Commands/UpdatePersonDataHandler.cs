namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

using Huwiyati.Domain.Events.Documents;

/// <summary>
/// Handles updating citizen biographical and demographic information linked to an active National ID Card.
/// </summary>
public class UpdatePersonDataHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDomainEventHandler<PersonDataUpdatedEvent> _personDataUpdatedEventHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePersonDataHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="fileStorageService">The file storage service for photo removal.</param>
    /// <param name="personDataUpdatedEventHandler">The domain event handler for person profile updates.</param>
    public UpdatePersonDataHandler(
        IApplicationDbContext context,
        IFileStorageService fileStorageService,
        IDomainEventHandler<PersonDataUpdatedEvent> personDataUpdatedEventHandler)
    {
        _context = context;
        _fileStorageService = fileStorageService;
        _personDataUpdatedEventHandler = personDataUpdatedEventHandler;
    }

    /// <summary>
    /// Updates person demographic information, swaps profile photos, and publishes update domain events.
    /// </summary>
    /// <param name="command">The command carrying updated person data.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the updated <see cref="NationalIdCardDto"/>.</returns>
    public async Task<ApiResponse<NationalIdCardDto>> UpdateAsync(
        UpdatePersonDataCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Find Person by National Number
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
                "Cannot update civil registry data for a deceased citizen.", statusCode: 400);
        }

        // 2. Project active National ID Card details using Select instead of Include for optimal performance
        var activeCardDto = await _context.NationalIdCards
            .Where(c => c.PersonId == person.Id && c.Status == NationalIdCardStatus.Active)
            .Select(c => new NationalIdCardDto
            {
                Id = c.Id,
                PersonId = c.PersonId,
                NationalNumber = person.NationalNumber,
                FullName = string.Empty, // Will be populated after updating Person properties
                IssuingBranchId = c.IssuingBranchId,
                BranchName = c.OrganizationBranch.BranchName,
                IssueDate = c.IssueDate,
                ExpiryDate = c.ExpiryDate,
                QrCodePayload = c.QrCodePayload,
                Status = c.Status.ToString(),
                CreatedAt = c.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (activeCardDto == null)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                "No active National ID Card found for this citizen.", statusCode: 404);
        }

        // 3. Update Person properties from command and delete old photo file if changed
        person.FirstName = command.FirstName;
        person.FatherName = command.FatherName;
        person.GrandfatherName = command.GrandfatherName;
        person.FamilyName = command.FamilyName;
        person.DateOfBirth = command.DateOfBirth;
        person.PlaceOfBirth = command.PlaceOfBirth;
        person.MaritalStatus = command.MaritalStatus;
        person.Governorate = command.Governorate;
        person.District = command.District;
        person.AddressDetails = command.AddressDetails;
        person.BloodGroup = command.BloodGroup;
        person.LastModifiedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(command.PhotoUrl))
        {
            if (!string.IsNullOrWhiteSpace(person.PhotoUrl) && person.PhotoUrl != command.PhotoUrl)
            {
                await _fileStorageService.DeletePhotoAsync(person.PhotoUrl, cancellationToken);
            }
            person.PhotoUrl = command.PhotoUrl;
        }

        _context.Persons.Update(person);
        await _context.SaveChangesAsync(cancellationToken);

        // 3.1 Trigger Domain Event to notify citizen
        var updateEvent = new PersonDataUpdatedEvent(person.Id);
        await _personDataUpdatedEventHandler.HandleAsync(updateEvent, cancellationToken);

        // 4. Update projected FullName and PhotoUrl, then return response DTO
        activeCardDto.FullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim();
        activeCardDto.PhotoUrl = person.PhotoUrl;

        return ApiResponse<NationalIdCardDto>.Success(
            activeCardDto, message: "National ID Card person data updated successfully.", statusCode: 200);
    }
}
