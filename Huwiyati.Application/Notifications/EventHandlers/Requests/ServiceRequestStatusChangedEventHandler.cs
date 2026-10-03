namespace Huwiyati.Application.Notifications.EventHandlers.Requests;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Requests;

public class ServiceRequestStatusChangedEventHandler : IDomainEventHandler<ServiceRequestStatusChangedEvent>
{
    private readonly IApplicationDbContext _context;

    public ServiceRequestStatusChangedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(ServiceRequestStatusChangedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تحديث حالة طلب الخدمة",
            Message = $"تغيرت حالة طلب الخدمة رقم {domainEvent.RequestNumber} إلى: {domainEvent.NewStatus}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
