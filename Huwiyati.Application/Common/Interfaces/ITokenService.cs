using Huwiyati.Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Huwiyati.Application.Common.Interfaces
{
    public interface ITokenService
    {
        GenerateTokenModel GenerateToken(
            Guid userId,
            string nationalNumber,
            string fullName,
            string accountStatus,
            IEnumerable<string> roles,
            Guid? organizationId = null,
            Guid? branchId = null);
    }
}
