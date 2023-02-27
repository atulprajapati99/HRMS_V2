using HRMS_V2.Core.Entities.Base;
using HRMS_V2.Core.Repositories.Base;
using HRMS_V2.Infrastructure.Data;

namespace HRMS_V2.Infrastructure.Repository.Base;

public class Repository<T> : RepositoryBase<T, int>, IRepository<T>
    where T : class, IEntityBase<int>
{
    public Repository(AdminContext context)
        : base(context)
    {
    }
}