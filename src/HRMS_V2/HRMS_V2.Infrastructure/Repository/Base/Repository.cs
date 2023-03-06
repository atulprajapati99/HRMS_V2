using HRMS_V2.Core.Entities;
using HRMS_V2.Core.Entities.Base;
using HRMS_V2.Core.Repositories.Base;
using HRMS_V2.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HRMS_V2.Infrastructure.Repository.Base;

public class Repository<T> : RepositoryBase<T, int>, IRepository<T>
    where T : class, IEntityBase<int>, IAuditable
{
    public Repository(AdminContext context, UserManager<AdminUser> userManager, IHttpContextAccessor httpContextAccessor)
        : base(context, userManager, httpContextAccessor)
    {
    }
}