namespace Huwiyati.Application.Notifications.EventHandlers.Organizations;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Organizations;

/// <summary>
/// Handles <see cref="EmployeeActivatedEvent"/> by notifying the employee that their account has been reactivated.
/// </summary>
public class EmployeeActivatedEventHandler : IDomainEventHandler<EmployeeActivatedEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeActivatedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public EmployeeActivatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the employee activated event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(EmployeeActivatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تفعيل حساب الموظف",
            Message = $"تم إعادة تفعيل حساب الموظف الخاص بك في فرع {domainEvent.BranchName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
