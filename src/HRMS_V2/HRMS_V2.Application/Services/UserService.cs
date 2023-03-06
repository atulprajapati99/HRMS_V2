using HRMS_V2.Application.Interfaces;
using HRMS_V2.Application.Models;
using HRMS_V2.Core.Entities;
using HRMS_V2.Core.Interfaces;
using HRMS_V2.Infrastructure.Securiy;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HRMS_V2.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AdminUser> _userManager;
        private readonly RoleManager<AdminRole> _roleManager;
        private readonly IAppLogger<UserService> _logger;

        public UserService(UserManager<AdminUser> userManager, RoleManager<AdminRole> roleManager, IAppLogger<UserService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<UserModel> AddAsync(UserModel request)
        {
            var user = new AdminUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                IsEnable = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return null;
            }

            request.Password = null;
            return request;
        }

        public async Task<RoleModel> AddRoleAsync(RoleModel request)
        {
            var result = await _roleManager.CreateAsync(new AdminRole { Name = request.Name.ToString() });           

            if (!result.Succeeded)
            {
                return null;
            }
            return request;
        }

        public async Task<bool> AddToRoleAsync(UserXRoleModel request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                throw new ApplicationException("User with this id is not exists");
            }

            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            if (role == null)
            {
                throw new ApplicationException("Role with this id is not exists");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddUserClaimAsync(RoleXClaimsModel request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                throw new ApplicationException("User with this id is not exists");
            }

            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            if (role == null)
            {
                throw new ApplicationException("Role with this id is not exists");
            }

            var allowedClaims = Claims.GetAllClaims();

            try
            {
                foreach (var userClaims in request.Claims)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(userClaims.Key, userClaims.Value));

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding claims to role - AdminAppService {ex.Message}", ex.StackTrace);
            }
            return true;
        }
    }
}
