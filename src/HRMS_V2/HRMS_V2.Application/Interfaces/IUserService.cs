using HRMS_V2.Application.Models;
using HRMS_V2.Application.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_V2.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> AddAsync(UserModel request);

        Task<RoleModel> AddRoleAsync(RoleModel request);

        Task<bool> AddToRoleAsync(UserXRoleModel request);

        Task<bool> AddUserClaimAsync(RoleXClaimsModel request);
    }
}
