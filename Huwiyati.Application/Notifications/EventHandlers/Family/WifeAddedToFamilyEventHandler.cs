namespace Huwiyati.Application.Notifications.EventHandlers.Family;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Family;

public class WifeAddedToFamilyEventHandler : IDomainEventHandler<WifeAddedToFamilyEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public WifeAddedToFamilyEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

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
