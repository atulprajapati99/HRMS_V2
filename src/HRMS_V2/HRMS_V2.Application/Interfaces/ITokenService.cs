using HRMS_V2.Application.Models.Authentication;

namespace HRMS_V2.Application.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> AuthenticateAsync(TokenRequestModel request, string ipAddress);
    }
}
