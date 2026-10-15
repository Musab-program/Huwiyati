namespace Huwiyati.Application.Notifications.DTOs;

/// <summary>
/// Represents data transfer object for an in-app user notification.
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the notification.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the notification header or title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed notification message text.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the notification has been read by the user.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Gets or sets the optional timestamp when the notification was marked as read.
    /// </summary>
    public DateTime? ReadAt { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp of the notification.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
