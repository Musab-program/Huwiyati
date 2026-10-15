namespace Huwiyati.Application.Notifications.DTOs;

/// <summary>
/// Represents paginated result wrapper for user notifications including unread counts.
/// </summary>
public class NotificationListResultDto
{
    /// <summary>
    /// Gets or sets the collection of notification items for the requested page.
    /// </summary>
    public List<NotificationDto> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the total count of unread notifications for the user.
    /// </summary>
    public int UnreadCount { get; set; }

    /// <summary>
    /// Gets or sets the total count of all notifications matching the query.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current 1-based page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; }
}
