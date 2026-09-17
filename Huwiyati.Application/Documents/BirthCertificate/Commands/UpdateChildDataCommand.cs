namespace Huwiyati.Application.Documents.BirthCertificate.Commands;

using Huwiyati.Domain.Enums;

public class UpdateChildDataCommand
{
    public Guid BirthCertificateId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PlaceOfBirth { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public string Governorate { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string AddressDetails { get; set; } = string.Empty;
}
