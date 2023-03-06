using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace HRMS_V2.Infrastructure.Securiy
{
    public static class Constants
    {
        public const string ApplicationCode = "hrms";
        public const string WildCardClaimValue = "*";
        public const string Permission = "permission";     
        public const string Role = "role";
        public const string ReadAccess = "read";
        public const string ManageAccess = "manage";
    }

    public enum BuiltInRole
    {
        [EnumMember]
        [Display(Name = "Employee")]
        [Description("Employee of the organisation.")]
        Empoloyee,

        [EnumMember]
        [Display(Name = "Manager")]
        [Description("Employee Manager for the emploees.")]
        Manager,

        [EnumMember]
        [Display(Name = "Human Resource")]
        [Description("Human Resource of the organisation.")]
        HR,

        [EnumMember]
        [Display(Name = "SuperAdmin")]
        [Description("SuperAdmin")]
        SuperAdmin
    }

    public static class Claims
    {
        public const string ClientId = "client_id";

        public const string Subject = "sub";

        public const string Name = "name";

        public const string Email = "email";

        public static readonly string SystemAdmin = "role/systemadmin";

        public static string ToReadClaim(this Permission permission)
        {
            return $"{Constants.Permission}/{Constants.ReadAccess}";
        }

        public static string ToManageClaim(this Permission permission)
        {
            return $"{Constants.Permission}/{Constants.ManageAccess}";
        }

        public static Permission ToPermissionFromClaim(this string claim)
        {
            if (claim.StartsWith($"{Constants.Permission}"))
            {
                var txt = claim.Substring(claim.LastIndexOf("/", StringComparison.Ordinal) + 1);

                if (Enum.TryParse(value: txt, ignoreCase: true, result: out Permission result))
                {
                    return result;
                }

                throw new Exception("Unsupported.");
            }
            throw new Exception("This extension method is only for permission claims");
        }

        public static IEnumerable<string> GetPermissionClaims(this IEnumerable<Permission> permissions)
        {
            foreach (var p in permissions)
            {
                yield return p.ToManageClaim();
                yield return p.ToReadClaim();
            }
        }

        public static IEnumerable<string> GetAllClaims()
        {
            foreach (var p in Enum.GetValues(typeof(Permission)).Cast<Permission>())
            {
                yield return p.ToReadClaim();
                yield return p.ToManageClaim();
            }
        }

        public static IEnumerable<Claim> SystemAdminClaims()
        {
            foreach (var p in Enum.GetValues(typeof(Permission)).Cast<Permission>())
            {
                yield return new Claim(p.ToManageClaim(), "*");
                yield return new Claim(p.ToReadClaim(), "*");
            }
        }
    }
}
