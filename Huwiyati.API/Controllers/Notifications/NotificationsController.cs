namespace Huwiyati.API.Controllers.Notifications;

using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Notifications.Commands;
using Huwiyati.Application.Notifications.DTOs;
using Huwiyati.Application.Notifications.Queries;
using Huwiyati.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = AppRoles.Citizen)]
public class NotificationsController : ControllerBase
{
    private readonly GetUserNotificationsHandler _getNotificationsHandler;
    private readonly MarkNotificationsAsReadHandler _markAsReadHandler;
    private readonly IIdentityService _identityService;

    public NotificationsController(
        GetUserNotificationsHandler getNotificationsHandler,
        MarkNotificationsAsReadHandler markAsReadHandler,
        IIdentityService identityService)
    {
        _getNotificationsHandler = getNotificationsHandler;
        _markAsReadHandler = markAsReadHandler;
        _identityService = identityService;
    }

    /// <summary>
    /// Get paginated notifications for the logged-in citizen/user.
    /// GET /api/v1/notifications
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<NotificationListResultDto>>> GetMyNotifications(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? onlyUnread = null,
        CancellationToken cancellationToken = default)
    {
        var userId = _identityService.GetUserIdFromClaims(User);
        if (userId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<NotificationListResultDto>.Failure("User ID claim is missing or invalid.", statusCode: 401));
        }

        var result = await _getNotificationsHandler.GetUserNotificationsAsync(
            userId,
            pageNumber,
            pageSize,
            onlyUnread,
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Mark a specific notification or all notifications as read.
    /// PATCH /api/v1/notifications (marks all unread as read)
    /// PATCH /api/v1/notifications?notificationId={id} (marks specific notification as read)
    /// </summary>
    [HttpPatch]
    public async Task<ActionResult<ApiResponse<string>>> MarkAsRead(
        [FromQuery] Guid? notificationId,
        CancellationToken cancellationToken = default)
    {
        var userId = _identityService.GetUserIdFromClaims(User);
        if (userId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<string>.Failure("User ID claim is missing or invalid.", statusCode: 401));
        }
        
        var result = await _markAsReadHandler.MarkAsReadAsync(
            userId,
            notificationId,
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }
}
