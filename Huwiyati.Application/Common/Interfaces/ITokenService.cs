using Huwiyati.Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Huwiyati.Application.Common.Interfaces;

using Huwiyati.Application.Common.Models;

/// <summary>
/// Defines JWT authentication and token generation services.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a signed JSON Web Token (JWT) encapsulating the user identity and assigned security claims.
    /// </summary>
    /// <param name="userId">The unique identifier of the user account.</param>
    /// <param name="nationalNumber">The citizen or employee national identification number.</param>
    /// <param name="fullName">The full name of the user.</param>
    /// <param name="accountStatus">The current status of the account.</param>
    /// <param name="roles">The collection of assigned roles.</param>
    /// <param name="organizationId">The optional organization identifier if the user is an employee.</param>
    /// <param name="branchId">The optional branch identifier if the user is an employee.</param>
    /// <returns>A populated <see cref="GenerateTokenModel"/> containing the access token string and expiration timestamp.</returns>
    GenerateTokenModel GenerateToken(
        Guid userId,
        string nationalNumber,
        string fullName,
        string accountStatus,
        IEnumerable<string> roles,
        Guid? organizationId = null,
        Guid? branchId = null);
}
