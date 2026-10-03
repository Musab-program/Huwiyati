namespace Huwiyati.Application.Notifications.EventHandlers.Organizations;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Organizations;

public class EmployeeAssignedEventHandler : IDomainEventHandler<EmployeeAssignedEvent>
{
    private readonly IApplicationDbContext _context;

    public EmployeeAssignedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(EmployeeAssignedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تعيين وظيفي جديد",
            Message = $"تم تعيينك كموظف رسمي في فرع {domainEvent.BranchName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
