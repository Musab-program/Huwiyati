namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Huwiyati.Domain.Enums;

// Command model containing all required Person fields to update a citizen's profile from Dashboard
public class UpdatePersonDataCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string GrandfatherName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PlaceOfBirth { get; set; } = string.Empty;
    public MaritalStatus MaritalStatus { get; set; }
    public string Governorate { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string AddressDetails { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public BloodGroup BloodGroup { get; set; }
}
