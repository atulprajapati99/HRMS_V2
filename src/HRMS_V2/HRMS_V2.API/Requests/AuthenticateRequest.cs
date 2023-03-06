using HRMS_V2.Application.Models;
using HRMS_V2.Application.Models.Authentication;
using MediatR;

namespace HRMS_V2.API.Requests
{
    public class AuthenticateRequest : IRequest<TokenResponse>
    {
        public TokenRequestModel TokenReuqest { get; set; }
    }
}
