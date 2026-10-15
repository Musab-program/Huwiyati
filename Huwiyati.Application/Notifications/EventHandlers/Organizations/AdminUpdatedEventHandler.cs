namespace Huwiyati.Application.Notifications.EventHandlers.Organizations;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Organizations;

/// <summary>
/// Handles <see cref="AdminUpdatedEvent"/> by creating an in-app notification when admin details or permissions are updated.
/// </summary>
public class AdminUpdatedEventHandler : IDomainEventHandler<AdminUpdatedEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminUpdatedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public AdminUpdatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the admin updated event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(AdminUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تحديث صلاحيات أدمن",
            Message = $"تم تحديث بيانات وصلاحيات الحساب الإداري الخاص بك في فرع {domainEvent.BranchName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
