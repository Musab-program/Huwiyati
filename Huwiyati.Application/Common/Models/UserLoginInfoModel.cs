using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Common.Models;

/// <summary>
/// Encapsulates authentication credentials validation results and user profile details upon login.
/// </summary>
public class UserLoginInfoModel
{
    /// <summary>
    /// Gets or sets a value indicating whether login verification succeeded.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user account.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the linked civil registry person identifier.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the current account status description (e.g. "Active", "Suspended").
    /// </summary>
    public string AccountStatus { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of role names assigned to the user.
    /// </summary>
    public List<string> Roles { get; set; } = new();
}
