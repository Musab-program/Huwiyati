namespace Huwiyati.Application.Authentication.DTOs;

using Huwiyati.Domain.Enums;

// DTO representing citizen account activation status details for staff UI
public class CitizenAccountStatusDto
{
    public Guid PersonId { get; set; }
    public string PersonFullName { get; set; } = string.Empty;
    public string NationalNumber { get; set; } = string.Empty;
    public bool HasRegisteredAccount { get; set; }
    public Guid? UserId { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public AccountStatus? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ActivatedAt { get; set; }
}
