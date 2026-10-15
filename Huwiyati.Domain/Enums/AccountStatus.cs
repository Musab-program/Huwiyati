namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the lifecycle and operational status of a user authentication account.
/// </summary>
public enum AccountStatus
{
    /// <summary>
    /// Account created but pending initial OTP phone or email verification.
    /// </summary>
    PendingActivation = 1,

    /// <summary>
    /// Account is active, verified, and permitted to perform system operations.
    /// </summary>
    Active = 2,

    /// <summary>
    /// Account is administratively suspended due to security violations or policy lock.
    /// </summary>
    Suspended = 3,

    /// <summary>
    /// Account was deactivated either by the citizen voluntarily or by administrative action.
    /// </summary>
    Deactivated = 4
}
