using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Authentication.Commands
{
    public class VerifyOTPCommand
    {
        public Guid UserId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
