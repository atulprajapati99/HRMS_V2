using HRMS_V2.Application.Models;
using HRMS_V2.Core.Paging;

namespace HRMS_V2.Application.Interfaces
{
    public interface IHolidayService
    {
        Task<IEnumerable<HolidayModel>> GetHolidayListAsync();
        Task<IPagedList<HolidayModel>> SearchHolidaysAsync(PageSearchArgs args);
        Task<HolidayModel> GetHolidayByIdAsync(int id);
        Task<HolidayModel> CreateHolidayAsync(HolidayModel holiday);
        Task UpdateHolidayAsync(HolidayModel holiday);
        Task DeleteHolidayByIdAsync(int Id);
    }
}
