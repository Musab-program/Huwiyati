namespace Huwiyati.Application.Notifications.EventHandlers.Documents;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Documents;

public class PersonDataUpdatedEventHandler : IDomainEventHandler<PersonDataUpdatedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public PersonDataUpdatedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

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
