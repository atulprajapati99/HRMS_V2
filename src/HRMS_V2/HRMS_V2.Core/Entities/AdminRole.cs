using HRMS_V2.Core.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace HRMS_V2.Core.Entities;

public class AdminRole : IdentityRole<int>, IEntityBase<int>
{
}