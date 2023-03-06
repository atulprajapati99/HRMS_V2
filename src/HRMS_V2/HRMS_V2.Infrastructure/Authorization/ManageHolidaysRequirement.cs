using HRMS_V2.Core.Entities;
using HRMS_V2.Infrastructure.Securiy;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HRMS_V2.Infrastructure.Authorization
{
    public class ManageHolidaysRequirement : IAuthorizationRequirement
    {
        public bool CanManageHolidays(AuthorizationHandlerContext context, Holiday holiday)
        {
            return context.User.HasClaim(Permission.Holiday.ToManageClaim(), holiday.Id.ToString());
        }        
    }

    public class ManageHolidaysRequirementHandler : AuthorizationHandler<ManageHolidaysRequirement, Holiday>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageHolidaysRequirement requirement, Holiday resource)
        {
            if (requirement.CanManageHolidays(context, resource))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
