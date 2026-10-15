namespace Huwiyati.Application.Notifications.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Handles updating the read status of user notifications.
/// </summary>
public class MarkNotificationsAsReadHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkNotificationsAsReadHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public MarkNotificationsAsReadHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Marks a specific notification or all unread notifications of a user as read.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="notificationId">The optional specific notification identifier to mark as read.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a descriptive confirmation message.</returns>
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
