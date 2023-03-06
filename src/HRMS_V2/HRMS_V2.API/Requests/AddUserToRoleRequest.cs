using HRMS_V2.Application.Models;
using MediatR;

namespace HRMS_V2.API.Requests
{
    public class AddUserToRoleRequest : IRequest<bool>
    {
        public UserXRoleModel UserXRoleModel { get; set; }
    }
}
