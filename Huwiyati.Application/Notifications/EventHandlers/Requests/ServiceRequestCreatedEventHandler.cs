namespace Huwiyati.Application.Notifications.EventHandlers.Requests;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Requests;

public class ServiceRequestCreatedEventHandler : IDomainEventHandler<ServiceRequestCreatedEvent>
{
    private readonly IApplicationDbContext _context;

    public ServiceRequestCreatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(ServiceRequestCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "طلب خدمة جديد",
            Message = $"تم تقديم طلب خدمة جديد بنجاح برقم {domainEvent.RequestNumber}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
