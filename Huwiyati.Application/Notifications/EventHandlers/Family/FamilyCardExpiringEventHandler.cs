using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Notifications.EventHandlers.Family
{
    public class FamilyCardExpiringEventHandler : IDomainEventHandler<DocumentsExpiringEvent>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public FamilyCardExpiringEventHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task HandleAsync(DocumentsExpiringEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var userId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.PersonId, cancellationToken);
            if (userId == null) return;
            var notification = new Notification
            {
                UserId = userId.Value,
                Title = "تنبيه قرب انتهاء السجل الأسري",
                Message = $"نود إشعاراتكم بأن البطاقة العائلية رقم ({domainEvent.DocumentNumber}) ستنتهي خلال {domainEvent.DaysRemaining} يوماً (بتاريخ {domainEvent.ExpirationDate:yyyy/MM/dd}). يرجى مراجعة مكاتب الأحوال المدنية أو تقديم طلب التجديد.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Notifications.AddAsync(notification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}