namespace Huwiyati.Application.Notifications.EventHandlers.Organizations;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Organizations;

public class EmployeeUpdatedEventHandler : IDomainEventHandler<EmployeeUpdatedEvent>
{
    private readonly IApplicationDbContext _context;

    public EmployeeUpdatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(EmployeeUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تحديث بيانات وظيفية",
            Message = $"تم تحديث بياناتك الوظيفية في فرع {domainEvent.BranchName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
