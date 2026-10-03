namespace Huwiyati.Application.Notifications.EventHandlers.Organizations;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Organizations;

public class AdminAssignedEventHandler : IDomainEventHandler<AdminAssignedEvent>
{
    private readonly IApplicationDbContext _context;

    public AdminAssignedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

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
