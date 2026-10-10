namespace Huwiyati.Application.Authentication.DTOs;

using Huwiyati.Domain.Enums;

// DTO representing registered application user account for SuperAdmin queries
public class RegisteredUserAccountDto
{
    public Guid UserId { get; set; }
    public Guid PersonId { get; set; }
    public string NationalNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public AccountStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ActivatedAt { get; set; }
}
