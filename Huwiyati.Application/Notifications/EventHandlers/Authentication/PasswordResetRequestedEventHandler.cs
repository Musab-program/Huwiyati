namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

public class PasswordResetRequestedEventHandler : IDomainEventHandler<PasswordResetRequestedEvent>
{
    private readonly IApplicationDbContext _context;

    public PasswordResetRequestedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(PasswordResetRequestedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "طلب إعادة تعيين كلمة المرور",
            Message = "تم تقديم طلب لإعادة تعيين كلمة المرور الخاصة بحسابك.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
