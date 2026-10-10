namespace Huwiyati.Application.Authentication.DTOs;

public class CitizenProfileDto
{
    public Guid UserId { get; set; }
    public Guid PersonId { get; set; }
    public string NationalNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string GrandfatherName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PlaceOfBirth { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string MaritalStatus { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public string Governorate { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string AddressDetails { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string AccountStatus { get; set; } = string.Empty;
    public bool HasActiveNationalIdCard { get; set; }
    public bool HasActivePassport { get; set; }
    public bool HasActiveFamilyCard { get; set; }
}
