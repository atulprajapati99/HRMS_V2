using HRMS_V2.Application.Models;
using MediatR;

namespace HRMS_V2.API.Requests
{
    public class AddRoleRequest : IRequest<RoleModel>
    {
        public RoleModel Role { get; set; }
    }
}
