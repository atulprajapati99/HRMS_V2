using HRMS_V2.Core.Entities;
using HRMS_V2.Core.Paging;
using HRMS_V2.Core.Repositories;
using HRMS_V2.Infrastructure.Data;
using HRMS_V2.Infrastructure.Paging;
using HRMS_V2.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace HRMS_V2.Infrastructure.Repository
{
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(AdminContext context, UserManager<AdminUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {
        }

        public async Task<Holiday> GetByIdAsyc(int id)
        {
            var holiday = await GetAsync(p => p.Id == id);
            return holiday.FirstOrDefault()!;
        }

        public async Task<IEnumerable<Holiday>> GetHolidayListAsync()
        {
            return await ListAllAsync();
        }

        public Task<IPagedList<Holiday>> SearchHolidaysAsync(PageSearchArgs args)
        {
            var query = Table;
            var orderByList = new List<Tuple<SortingOption, Expression<Func<Holiday, object>>>>();

            if (args.SortingOptions != null)
            {
                foreach (var sortingOption in args.SortingOptions)
                {
                    switch (sortingOption.Field)
                    {
                        case "id":
                            orderByList.Add(new Tuple<SortingOption, Expression<Func<Holiday, object>>>(sortingOption, p => p.Id));
                            break;
                        case "name":
                            orderByList.Add(new Tuple<SortingOption, Expression<Func<Holiday, object>>>(sortingOption, p => p.Name));
                            break;
                    }
                }
            }

            if (orderByList.Count == 0)
            {
                orderByList.Add(new Tuple<SortingOption, Expression<Func<Holiday, object>>>(new SortingOption { Direction = SortingOption.SortingDirection.ASC }, p => p.Id));
            }

            var filterList = new List<Tuple<FilteringOption, Expression<Func<Holiday, bool>>>>();

            if (args.FilteringOptions != null)
            {
                foreach (var filteringOption in args.FilteringOptions)
                {
                    switch (filteringOption.Field)
                    {
                        case "id":
                            filterList.Add(new Tuple<FilteringOption, Expression<Func<Holiday, bool>>>(filteringOption, p => p.Id == (int)filteringOption.Value));
                            break;
                        case "name":
                            filterList.Add(new Tuple<FilteringOption, Expression<Func<Holiday, bool>>>(filteringOption, p => p.Name.Contains((string)filteringOption.Value)));
                            break;
                    }
                }
            }

            var holidayPagedList = new PagedList<Holiday>(query, new PagingArgs { PageIndex = args.PageIndex, PageSize = args.PageSize, PagingStrategy = args.PagingStrategy }, orderByList, filterList);

            return Task.FromResult<IPagedList<Holiday>>(holidayPagedList);
        }
    }
}
