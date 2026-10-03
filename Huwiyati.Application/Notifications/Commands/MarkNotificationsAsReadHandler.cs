namespace Huwiyati.Application.Notifications.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

public class MarkNotificationsAsReadHandler
{
    private readonly IApplicationDbContext _context;

    public MarkNotificationsAsReadHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<string>> MarkAsReadAsync(
        Guid userId,
        Guid? notificationId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead);

        if (notificationId.HasValue && notificationId.Value != Guid.Empty)
        {
            query = query.Where(n => n.Id == notificationId.Value);
        }

        var unreadNotifications = await query.ToListAsync(cancellationToken);

        if (!unreadNotifications.Any())
        {
            return ApiResponse<string>.Success("No unread notifications to update.");
        }

        var now = DateTime.UtcNow;
        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadAt = now;
        }

        await _context.SaveChangesAsync(cancellationToken);

        var message = notificationId.HasValue
            ? "Notification marked as read successfully."
            : $"{unreadNotifications.Count} notification(s) marked as read successfully.";

        return ApiResponse<string>.Success(message);
    }
}
