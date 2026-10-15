namespace Huwiyati.Application.Notifications.EventHandlers.Requests;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Requests;

/// <summary>
/// Handles <see cref="ServiceRequestStatusChangedEvent"/> by notifying the applicant when the request status changes.
/// </summary>
public class ServiceRequestStatusChangedEventHandler : IDomainEventHandler<ServiceRequestStatusChangedEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRequestStatusChangedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public ServiceRequestStatusChangedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the service request status changed event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
