using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Common.Models
{
    public class UserLoginInfoModel
    {
        public bool Succeeded { get; set; }
        public Guid UserId { get; set; }
        public Guid PersonId { get; set; }
        public string AccountStatus { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
