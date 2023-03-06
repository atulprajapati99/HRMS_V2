using HRMS_V2.Application.Interfaces;
using HRMS_V2.Application.Mapper;
using HRMS_V2.Application.Models;
using HRMS_V2.Core.Entities;
using HRMS_V2.Core.Interfaces;
using HRMS_V2.Core.Paging;
using HRMS_V2.Core.Repositories;
using HRMS_V2.Infrastructure.Paging;
using System.Diagnostics.Metrics;

namespace HRMS_V2.Application.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IAppLogger<HolidayService> _logger;

        public HolidayService(IHolidayRepository holidayRepository, IAppLogger<HolidayService> logger)
        {
            _holidayRepository = holidayRepository;
            _logger = logger;
        }

        public async Task<HolidayModel> CreateHolidayAsync(HolidayModel holiday)
        {
            var existingHoliday = await _holidayRepository.GetByIdAsync(holiday.Id);
            if (existingHoliday != null)
            {
                throw new ApplicationException("Holiday with this id already exists");
            }

            var newHoliday = ObjectMapper.Mapper.Map<Holiday>(holiday);
            newHoliday = await _holidayRepository.SaveAsync(newHoliday);

            _logger.LogInformation("Entity successfully added - AdminAppService");

            var newHolidayModel = ObjectMapper.Mapper.Map<HolidayModel>(newHoliday);
            return newHolidayModel;
        }

        public async Task DeleteHolidayByIdAsync(int Id)
        {
            var existingHoliday = await _holidayRepository.GetByIdAsync(Id);
            if (existingHoliday == null)
            {
                throw new ApplicationException("Holiday with this id is not exists");
            }

            existingHoliday.RecordStatus = "0";

            await _holidayRepository.SaveAsync(existingHoliday);          

            _logger.LogInformation("Entity successfully deleted");
        }

        public async Task<HolidayModel> GetHolidayByIdAsync(int id)
        {
            var holiday = await _holidayRepository.GetByIdAsync(id);

            var holidayModel = ObjectMapper.Mapper.Map<HolidayModel>(holiday);

            return holidayModel;
        }

        public async Task<IEnumerable<HolidayModel>> GetHolidayListAsync()
        {
            var holidayList = await _holidayRepository.ListAllAsync();

            var holidayModels = ObjectMapper.Mapper.Map<IEnumerable<HolidayModel>>(holidayList);

            return holidayModels;
        }        

        public async Task<IPagedList<HolidayModel>> SearchHolidaysAsync(PageSearchArgs args)
        {
            var holidayPagedList = await _holidayRepository.SearchHolidaysAsync(args);

            //TODO: PagedList<TSource> will be mapped to PagedList<TDestination>;
            var holidayModel = ObjectMapper.Mapper.Map<List<HolidayModel>>(holidayPagedList.Items);

            var holidayModelPagedList = new PagedList<HolidayModel>(
                holidayPagedList.PageIndex,
                holidayPagedList.PageSize,
                holidayPagedList.TotalCount,
                holidayPagedList.TotalPages,
                holidayModel);

            return holidayModelPagedList;
        }

        public async Task UpdateHolidayAsync(HolidayModel holiday)
        {
            var existingHoliday = await _holidayRepository.GetByIdAsync(holiday.Id);
            if (existingHoliday == null)
            {
                throw new ApplicationException("Holiday with this id is not exists");
            }

            existingHoliday.Name = holiday.Name;
            existingHoliday.Description = holiday.Description;
            existingHoliday.Day = holiday.Day;        
            existingHoliday.RecordStatus = holiday.RecordStatus;

            await _holidayRepository.SaveAsync(existingHoliday);

            _logger.LogInformation("Entity successfully updated - AdminAppService");
        }
    }
}
