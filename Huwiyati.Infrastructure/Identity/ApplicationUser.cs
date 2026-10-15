namespace Huwiyati.Infrastructure.Identity;

using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Represents the application user identity account extending ASP.NET Core Identity.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// Gets or sets the foreign key referencing the associated civil registry person record.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property to the associated civil registry person record.
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of user devices registered for authentication and security tracking.
    /// </summary>
    public ICollection<UserDevice> Devices { get; set; } = new List<UserDevice>();

    /// <summary>
    /// Gets or sets the lifecycle status of the user account.
    /// </summary>
    public AccountStatus Status { get; set; } = AccountStatus.PendingActivation;

    /// <summary>
    /// Gets or sets the timestamp when the user account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp when the account was activated.
    /// </summary>
    public DateTime? ActivatedAt { get; set; }

    /// <summary>
    /// Gets or sets the navigation property to the associated employee record if the user is a staff member.
    /// </summary>
    public Employee? Employee { get; set; }
}
