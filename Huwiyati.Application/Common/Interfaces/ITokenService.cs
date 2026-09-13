using Huwiyati.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Common.Interfaces
{
    public interface ITokenService
    {
        GenerateTokenModel GenerateToken(
            Guid userId,
            string nationalNumber,
            string fullName,
            string accountStatus,
            IEnumerable<string> roles);
    }
}
