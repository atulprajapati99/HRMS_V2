using HRMS_V2.Core.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace HRMS_V2.Core.Entities;

public class AdminUser : IdentityUser<int>, IEntityBase<int>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime LastLoginTime { get; set; }

    public bool IsWorking { get; set; }
}