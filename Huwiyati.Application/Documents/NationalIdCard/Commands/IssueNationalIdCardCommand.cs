namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Huwiyati.Domain.Enums;

// Command model carrying required data to issue a new National ID Card (ExpiryDate is fixed 10 years automatically)
public class IssueNationalIdCardCommand
{
    // Case A: Optional existing Person identifier
    public Guid? PersonId { get; set; }

    // Case B: Person fields required if PersonId is null (New Person Creation)
    public string? FirstName { get; set; }
    public string? FatherName { get; set; }
    public string? GrandfatherName { get; set; }
    public string? FamilyName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? PlaceOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? Nationality { get; set; }
    public MaritalStatus? MaritalStatus { get; set; }
    public string? Governorate { get; set; }
    public string? District { get; set; }
    public string? AddressDetails { get; set; }
    public string? PhotoUrl { get; set; }
    public BloodGroup? BloodGroup { get; set; }

    // Card Specific Parameters
    public Guid IssuingBranchId { get; set; }
    public DateOnly? IssueDate { get; set; }
}