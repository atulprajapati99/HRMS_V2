using HRMS_V2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_V2.Application.Models.Authentication
{
    public class TokenResponse
    {
        public TokenResponse(AdminUser user,                           
                            string token
                           )
        {
            Id = user.Id;
            FullName = user.FullName;
            EmailAddress = user.Email;
            Token = token;            
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Token { get; set; }        
    }
}
