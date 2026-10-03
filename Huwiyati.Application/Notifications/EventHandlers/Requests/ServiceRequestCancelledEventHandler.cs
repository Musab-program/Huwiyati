namespace Huwiyati.Application.Notifications.EventHandlers.Requests;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Requests;

public class ServiceRequestCancelledEventHandler : IDomainEventHandler<ServiceRequestCancelledEvent>
{
    private readonly IApplicationDbContext _context;

    public ServiceRequestCancelledEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(ServiceRequestCancelledEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "إلغاء طلب الخدمة",
            Message = $"تم إلغاء طلب الخدمة رقم {domainEvent.RequestNumber} بنجاح.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
