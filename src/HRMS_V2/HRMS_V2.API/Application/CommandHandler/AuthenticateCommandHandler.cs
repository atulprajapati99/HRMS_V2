using HRMS_V2.API.Requests;
using HRMS_V2.Application.Interfaces;
using HRMS_V2.Application.Models.Authentication;
using MediatR;
using HRMS_V2.API.Customization;

namespace HRMS_V2.API.Application.CommandHandler
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateRequest, TokenResponse>
    {
        private readonly ITokenService _tokenService;
        private readonly HttpContext _httpContext;

        public AuthenticateCommandHandler(ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            this._tokenService = tokenService;
            this._httpContext = (httpContextAccessor != null) ? httpContextAccessor.HttpContext : throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public async Task<TokenResponse> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
        {
            string ipAddress = _httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            TokenResponse tokenResponse = await _tokenService.AuthenticateAsync(request.TokenReuqest, ipAddress);

            if (tokenResponse == null)
            {
                throw new InvalidCredentialsException("Invalid email or password.");
            }

            return tokenResponse;
        }
    }
}
