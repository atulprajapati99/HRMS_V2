using HRMS_V2.Core.Entities.Base;

namespace HRMS_V2.Core.Repositories.Base;

public interface IEnumRepository<T> : IRepositoryBase<T, int> where T : IEntityBase<int>
{
}