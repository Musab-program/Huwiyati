namespace Huwiyati.Infrastructure.Services;

using System.Security.Claims;
using Huwiyati.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

/// <summary>
/// Service providing access to the current authenticated user's identity details from HTTP context claims.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the unique user identifier from the current request claims, or <c>null</c> if unauthenticated.
    /// </summary>
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
