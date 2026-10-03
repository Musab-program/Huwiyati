namespace Huwiyati.Application.Notifications.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Notifications.DTOs;

public class GetUserNotificationsHandler
{
    private readonly IApplicationDbContext _context;

    public GetUserNotificationsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<NotificationListResultDto>> GetUserNotificationsAsync(
        Guid userId,
        int pageNumber = 1,
        int pageSize = 10,
        bool? onlyUnread = null,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var baseQuery = _context.Notifications
            .Where(n => n.UserId == userId);

        if (onlyUnread.HasValue && onlyUnread.Value)
        {
            baseQuery = baseQuery.Where(n => !n.IsRead);
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);
        var unreadCount = await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);

        var notifications = await baseQuery
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                ReadAt = n.ReadAt,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var result = new NotificationListResultDto
        {
            Items = notifications,
            UnreadCount = unreadCount,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return ApiResponse<NotificationListResultDto>.Success(result, "Notifications retrieved successfully.");
    }
}
