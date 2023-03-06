using HRMS_V2.Application.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HRMS_V2.API.Requests
{
    public class AddUserRequest : IRequest<UserModel>
    {
        public UserModel User { get; set; }
    }
}
