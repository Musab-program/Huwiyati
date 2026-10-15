namespace Huwiyati.Application.Notifications.EventHandlers.Requests;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Requests;

/// <summary>
/// Handles <see cref="ServiceRequestCancelledEvent"/> by notifying the applicant of the cancellation.
/// </summary>
public class ServiceRequestCancelledEventHandler : IDomainEventHandler<ServiceRequestCancelledEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRequestCancelledEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public ServiceRequestCancelledEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the service request cancelled event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
