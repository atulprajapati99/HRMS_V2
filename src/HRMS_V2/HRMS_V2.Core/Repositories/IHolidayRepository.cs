using HRMS_V2.Core.Entities;
using HRMS_V2.Core.Paging;
using HRMS_V2.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_V2.Core.Repositories
{
    public interface IHolidayRepository : IRepository<Holiday>
    {
        Task<IEnumerable<Holiday>> GetHolidayListAsync();

        Task<IPagedList<Holiday>> SearchHolidaysAsync(PageSearchArgs args);       

        Task<Holiday> GetByIdAsyc(int id);
    }
}
