using HRMS_V2.API.Requests;
using HRMS_V2.Core.Configuration;
using HRMS_V2.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS_V2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AdminSettings _AdminSettings;
        private readonly SignInManager<AdminUser> _signInManager;
        private readonly UserManager<AdminUser> _userManager;

        public AccountController(SignInManager<AdminUser> signInManager,
         UserManager<AdminUser> userManager,
         IOptions<AdminSettings> options)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _AdminSettings = options.Value;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                    if (result.Succeeded)
                    {
                        // Create the token
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_AdminSettings.Tokens.Key));
                        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                          _AdminSettings.Tokens.Issuer,
                          _AdminSettings.Tokens.Audience,
                          claims,
                          expires: DateTime.Now.AddMinutes(30),
                          signingCredentials: signingCredentials);

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created("", results);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Unauthorized();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AdminUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email          
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            request.Password = null;
            return Created("", request);
        }
    }
}
