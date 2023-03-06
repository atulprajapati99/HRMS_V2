using HRMS_V2.Application.Models;
using MediatR;

namespace HRMS_V2.API.Requests
{
    public class AddRoleToClaimRequest : IRequest<bool>
    {
        public RoleXClaimsModel RoleClaims { get; set; }
    }
}
