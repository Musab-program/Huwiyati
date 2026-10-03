namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

public class AccountReactivatedEventHandler : IDomainEventHandler<AccountReactivatedEvent>
{
    private readonly IApplicationDbContext _context;

    public AccountReactivatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(AccountReactivatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "إعادة تفعيل الحساب",
            Message = "تم إعادة تفعيل حسابك الرقمي بنجاح.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
