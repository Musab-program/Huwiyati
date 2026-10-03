namespace Huwiyati.Application.Notifications.DTOs;

public class NotificationListResultDto
{
    public List<NotificationDto> Items { get; set; } = new();
    public int UnreadCount { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
