namespace Huwiyati.Infrastructure.Identity;

using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    // Foreign Key and Unidirectional Navigation Property to Person entity in Domain
    public Guid PersonId { get; set; }
    public Person Person { get; set; } = null!;

    // Navigation Property to UserDevices in Domain
    public ICollection<UserDevice> Devices { get; set; } = new List<UserDevice>();

    // Custom Account Status and Audit Properties
    public AccountStatus Status { get; set; } = AccountStatus.PendingActivation;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ActivatedAt { get; set; }

    // Navigation Property to Employee entity in Domain (if applicable) expected to be null if the user is not an employee
    public Employee? Employee { get; set; }

    ///Notes:
    ///refresh token must be stored in DB
    ///Add data protection for personal
}
