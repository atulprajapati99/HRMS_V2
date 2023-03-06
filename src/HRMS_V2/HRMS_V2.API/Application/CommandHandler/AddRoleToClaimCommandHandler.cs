using HRMS_V2.API.Requests;
using HRMS_V2.Application.Interfaces;
using HRMS_V2.Application.Models;
using MediatR;

namespace HRMS_V2.API.Application.CommandHandler
{
    public class AddRoleToClaimCommandHandler : IRequestHandler<AddRoleToClaimRequest, bool>
    {
        private readonly IUserService _userService;

        public AddRoleToClaimCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(AddRoleToClaimRequest request, CancellationToken cancellationToken)
        {
            return await _userService.AddUserClaimAsync(request.RoleClaims);        
        }
    }
}
