using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_V2.Infrastructure.Securiy
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool CanManage(this ClaimsPrincipal user, Permission claimType, int tenantId)
        {
            var userClaimValues = user.Claims.Where(c =>
                    c.Type.Equals(claimType.ToManageClaim(), StringComparison.InvariantCultureIgnoreCase))
                .Select(c => c.Value);

            return userClaimValues.Any(c =>
            {                
                int.TryParse(c, out int id);
                return id == tenantId;
            });
        }

        public static bool CanRead(this ClaimsPrincipal user, Permission claimType, int tenantId)
        {
            var userClaimValues = (user.Claims.Where(c =>
                    c.Type.Equals(claimType.ToReadClaim(),
                        StringComparison.InvariantCultureIgnoreCase))
                .Union(user.Claims.Where(c =>
                    c.Type.Equals(claimType.ToManageClaim(),
                        StringComparison.InvariantCultureIgnoreCase)))).Select(c => c.Value);

            return userClaimValues.Any(c =>
            {
                if (c == Constants.WildCardClaimValue)
                {
                    return true;
                }

                int.TryParse(c, out int id);
                return id == tenantId;
            });
        }

        /// <summary>
        /// Determines if the principal is a system admin.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static bool IsSystemAdmin(this ClaimsPrincipal principal)
        {
            return principal.HasClaim(c => c.Type.Contains(Claims.SystemAdmin));
        }
    }
}
