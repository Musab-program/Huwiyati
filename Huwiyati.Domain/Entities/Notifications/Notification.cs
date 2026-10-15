namespace Huwiyati.Domain.Entities.Notifications;

using Huwiyati.Domain.Common;

/// <summary>
/// Represents an in-system notification sent to a citizen or administrative user.
/// </summary>
public class Notification : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the target recipient user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the brief title of the notification.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed notification message content.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the recipient has read the notification.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time when the notification was marked as read.
    /// </summary>
    public DateTime? ReadAt { get; set; }
}
