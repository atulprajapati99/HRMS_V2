using HRMS_V2.API.Requests;
using HRMS_V2.Application.Interfaces;
using MediatR;

namespace HRMS_V2.API.Application.CommandHandler
{
    public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleRequest, bool>
    {
        private readonly IUserService _userService;

        public AddUserToRoleCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(AddUserToRoleRequest request, CancellationToken cancellationToken)
        {
            return await _userService.AddToRoleAsync(request.UserXRoleModel);
        }
    }
}
