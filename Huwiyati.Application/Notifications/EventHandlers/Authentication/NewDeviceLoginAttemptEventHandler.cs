namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

public class NewDeviceLoginAttemptEventHandler : IDomainEventHandler<NewDeviceLoginAttemptEvent>
{
    private readonly IApplicationDbContext _context;

    public NewDeviceLoginAttemptEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(NewDeviceLoginAttemptEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "محاولة دخول من جهاز جديد",
            Message = $"هناك محاولة تسجيل دخول لحسابك من جهاز جديد: {domainEvent.DeviceName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
