using HRMS_V2.API.Requests;
using HRMS_V2.Application.Interfaces;
using HRMS_V2.Application.Models;
using MediatR;

namespace HRMS_V2.API.Application.CommandHandler
{
    public class AddRoleCommandHandler : IRequestHandler<AddRoleRequest, RoleModel>
    {
        private readonly IUserService _userService;

        public AddRoleCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<RoleModel> Handle(AddRoleRequest request, CancellationToken cancellationToken)
        {
            RoleModel addRoleResponse = await _userService.AddRoleAsync(request.Role);

            if (addRoleResponse == null)
            {
                throw new Exception("");
            }

            return addRoleResponse;
        }
    }
}
