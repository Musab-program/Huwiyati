using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to verify the initial registration OTP code sent to the citizen.
/// </summary>
public class VerifyOTPCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the user account.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the 6-digit verification code string.
    /// </summary>
    public string Code { get; set; } = string.Empty;
}
