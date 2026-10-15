namespace Huwiyati.Domain.Constants;

/// <summary>
/// Defines application role constants utilized throughout role-based access control (RBAC) and authorization policies.
/// </summary>
public static class AppRoles
{
    /// <summary>
    /// Represents the public citizen user role with access to self-service personal documents and requests.
    /// </summary>
    public const string Citizen = "Citizen";

    /// <summary>
    /// Represents the organization staff employee role authorized to review and process civil service requests.
    /// </summary>
    public const string Employee = "Employee";

    /// <summary>
    /// Represents the branch administration role with authority over branch employees and operations.
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// Represents the sovereign super-administrator role with platform-wide governance and organization management authority.
    /// </summary>
    public const string SuperAdmin = "SuperAdmin";
}