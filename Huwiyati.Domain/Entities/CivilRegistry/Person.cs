namespace Huwiyati.Domain.Entities.CivilRegistry;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Enums;

public class Person : AuditableEntity
{
    public string NationalNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string GrandfatherName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PlaceOfBirth { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public MaritalStatus MaritalStatus { get; set; }
    public string Governorate { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string AddressDetails { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public BloodGroup? BloodGroup { get; set; }
    public PersonStatus PersonStatus { get; set; }
}
