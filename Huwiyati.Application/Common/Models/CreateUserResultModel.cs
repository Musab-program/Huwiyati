using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Common.Models
{
    public class CreateUserResultModel
    {
        public bool Succeeded { get; set; }
        public Guid UserId { get; set; }
        public List<string> Errors { get; set; } = new();

    }
}
