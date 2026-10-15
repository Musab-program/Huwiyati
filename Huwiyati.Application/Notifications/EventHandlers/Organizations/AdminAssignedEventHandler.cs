namespace Huwiyati.Application.Notifications.EventHandlers.Organizations;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Organizations;

/// <summary>
/// Handles <see cref="AdminAssignedEvent"/> by creating an in-app notification when an admin role is assigned.
/// </summary>
public class AdminAssignedEventHandler : IDomainEventHandler<AdminAssignedEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminAssignedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public AdminAssignedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the admin assigned event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(AdminAssignedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تعيين صلاحيات أدمن",
            Message = $"تم منحك صلاحيات مدير لفرع {domainEvent.BranchName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
