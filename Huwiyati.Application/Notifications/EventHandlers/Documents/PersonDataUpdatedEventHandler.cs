namespace Huwiyati.Application.Notifications.EventHandlers.Documents;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Documents;

/// <summary>
/// Handles <see cref="PersonDataUpdatedEvent"/> by dispatching an in-app notification confirming the personal data update.
/// </summary>
public class PersonDataUpdatedEventHandler : IDomainEventHandler<PersonDataUpdatedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonDataUpdatedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity service to resolve user identifiers.</param>
    public PersonDataUpdatedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Handles the person data updated event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(PersonDataUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var userId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.PersonId, cancellationToken);
        if (userId == null) return;

        var notification = new Notification
        {
            UserId = userId.Value,
            Title = "تحديث البيانات الشخصية",
            Message = "تم تحديث بياناتك الشخصية بنجاح في السجل المدني.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
