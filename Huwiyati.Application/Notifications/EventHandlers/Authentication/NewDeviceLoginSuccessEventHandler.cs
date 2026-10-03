namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

public class NewDeviceLoginSuccessEventHandler : IDomainEventHandler<NewDeviceLoginSuccessEvent>
{
    private readonly IApplicationDbContext _context;

    public NewDeviceLoginSuccessEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(NewDeviceLoginSuccessEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تسجيل دخول من جهاز جديد",
            Message = $"تم تسجيل الدخول بنجاح لحسابك من جهاز جديد: {domainEvent.DeviceName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
