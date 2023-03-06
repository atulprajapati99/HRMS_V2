using HRMS_V2.Application.Interfaces;
using HRMS_V2.Application.Models.Authentication;
using HRMS_V2.Core.Configuration;
using HRMS_V2.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS_V2.Application.Services
{
    public class TokenService : ITokenService
    {

        private readonly SignInManager<AdminUser> _signInManager;
        private readonly UserManager<AdminUser> _userManager;
        private readonly AdminSettings _token;       

        public TokenService(
            UserManager<AdminUser> userManager,
            SignInManager<AdminUser> signInManager,
            IOptions<AdminSettings> tokenOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = tokenOptions.Value;            
        }

        public async Task<TokenResponse> AuthenticateAsync(TokenRequestModel request, string ipAddress)
        {
            if (await IsValidUser(request.Email, request.Password))
            {
                AdminUser user = await _userManager.FindByEmailAsync(request.Email);

                if (user != null && user.IsEnable)
                {
                    //string role = (await _userManager.GetRolesAsync(user))[0];
                    string jwtToken = await GenerateJwtToken(user);

                    user.LastLoginTime = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                    return new TokenResponse(user,
                                             jwtToken
                                             );
                }
            }

            return null;
        }

        private async Task<bool> IsValidUser(string email, string password)
        {
            AdminUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // Username or password was incorrect.
                return false;
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, password, true, false);

            return signInResult.Succeeded;
        }

        private async Task<string> GenerateJwtToken(AdminUser user)
        {
            //string role = (await _userManager.GetRolesAsync(user))[0];
            byte[] secret = Encoding.ASCII.GetBytes(_token.Tokens.Key);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Issuer = _token.Tokens.Issuer,
                Audience = _token.Tokens.Audience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("FullName", $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    //new Claim(ClaimTypes.Role, role)    
                     //new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                     //       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     //       new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_token.Tokens.Expiry),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
