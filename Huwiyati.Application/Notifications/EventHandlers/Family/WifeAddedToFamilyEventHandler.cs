namespace Huwiyati.Application.Notifications.EventHandlers.Family;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Family;

/// <summary>
/// Handles <see cref="WifeAddedToFamilyEvent"/> by creating in-app notifications for both the husband and the wife.
/// </summary>
public class WifeAddedToFamilyEventHandler : IDomainEventHandler<WifeAddedToFamilyEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WifeAddedToFamilyEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity service to resolve user identifiers.</param>
    public WifeAddedToFamilyEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Handles the wife added to family event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(WifeAddedToFamilyEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        // 1. Notify Husband (Head of Family) if user account exists
        var husbandUserId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.HeadOfFamilyPersonId, cancellationToken);
        if (husbandUserId != null)
        {
            var husbandNotification = new Notification
            {
                UserId = husbandUserId.Value,
                Title = "إضافة زوجة للبطاقة العائلية",
                Message = $"تم إضافة الزوجة {domainEvent.WifeName} إلى البطاقة العائلية بنجاح.",
                IsRead = false,
                CreatedAt = now
            };
            await _context.Notifications.AddAsync(husbandNotification, cancellationToken);
        }

        // 2. Notify Wife if user account exists
        var wifeUserId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.WifePersonId, cancellationToken);
        if (wifeUserId != null)
        {
            var wifeNotification = new Notification
            {
                UserId = wifeUserId.Value,
                Title = "إضافة إلى البطاقة العائلية",
                Message = "تم إضافتك كزوجة إلى البطاقة العائلية بنجاح.",
                IsRead = false,
                CreatedAt = now
            };
            await _context.Notifications.AddAsync(wifeNotification, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
