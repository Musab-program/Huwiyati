namespace Huwiyati.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Enums;

public class ApplicationUser : IdentityUser<Guid>
{
    // Foreign Key and Unidirectional Navigation Property to Person entity in Domain
    public Guid PersonId { get; set; }
    public Person Person { get; set; } = null!;

    // Custom Account Status and Audit Properties
    public AccountStatus Status { get; set; } = AccountStatus.PendingActivation;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ActivatedAt { get; set; }

    ///Notes:
    ///refresh token must be stored in DB
    ///Add data protection for personal
}
