namespace Huwiyati.Application.Common.Models;

using Huwiyati.Domain.Enums;

public class UserAccountDetailsModel
{
    public Guid UserId { get; set; }
    public Guid PersonId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public AccountStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ActivatedAt { get; set; }
}
