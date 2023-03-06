using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_V2.Application.Models
{
    public class RoleXClaimsModel
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public IDictionary<string, string> Claims { get; set; }
    }
}
