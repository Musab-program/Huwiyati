namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

public class AccountDeactivatedEventHandler : IDomainEventHandler<AccountDeactivatedEvent>
{
    private readonly IApplicationDbContext _context;

    public AccountDeactivatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(AccountDeactivatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "إلغاء تفعيل الحساب",
            Message = "تم إلغاء تفعيل حسابك الرقمي في تطبيق هويتي.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
