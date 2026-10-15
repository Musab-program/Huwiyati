namespace Huwiyati.Application.Notifications.EventHandlers.Requests;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Requests;

/// <summary>
/// Handles <see cref="ServiceRequestCreatedEvent"/> by dispatching an acknowledgement notification to the applicant.
/// </summary>
public class ServiceRequestCreatedEventHandler : IDomainEventHandler<ServiceRequestCreatedEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRequestCreatedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public ServiceRequestCreatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the service request created event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
