namespace Huwiyati.Application.Notifications.EventHandlers.Family;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Family;

public class FamilyMemberStatusChangedEventHandler : IDomainEventHandler<FamilyMemberStatusChangedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public FamilyMemberStatusChangedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task HandleAsync(FamilyMemberStatusChangedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var userId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.HeadOfFamilyPersonId, cancellationToken);
        if (userId == null) return;

        var notification = new Notification
        {
            UserId = userId.Value,
            Title = "تحديث حالة فرد في العائلة",
            Message = $"تم تحديث حالة السجل العائلي للفرد {domainEvent.MemberName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
