using HRMS_V2.API.Customization;
using HRMS_V2.API.Requests;
using HRMS_V2.Application.Interfaces;
using HRMS_V2.Application.Models;
using HRMS_V2.Application.Models.Authentication;
using HRMS_V2.Application.Services;
using MediatR;
using System.Net;

namespace HRMS_V2.API.Application.CommandHandler
{
    public class AddUserCommandHandler : IRequestHandler<AddUserRequest, UserModel>
    {
        private readonly IUserService _userService;

        public AddUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserModel> Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            UserModel addUserResponse = await _userService.AddAsync(request.User);

            if (addUserResponse == null)
            {
                throw new ApplicationException("");
            }

            return addUserResponse;
        }
    }
}
