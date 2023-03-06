using HRMS_V2.Core.Entities;
using HRMS_V2.Infrastructure.Securiy;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HRMS_V2.Infrastructure.Authorization
{
    public class ReadHolidayRequirement : IAuthorizationRequirement
    {
        public bool CanReadHolidays(AuthorizationHandlerContext context, Holiday holiday)
        {
            return context.User.CanRead(Permission.Holiday, holiday.Id);
        }        
    }

    public class ReadHolidaysRequirementHandler : AuthorizationHandler<ReadHolidayRequirement, Holiday>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ReadHolidayRequirement requirement, Holiday resource)
        {
            if (requirement.CanReadHolidays(context, resource))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
