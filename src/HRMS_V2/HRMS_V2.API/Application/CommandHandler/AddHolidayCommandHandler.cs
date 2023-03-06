using HRMS_V2.API.Requests;
using HRMS_V2.Application.Interfaces;
using HRMS_V2.Application.Models;
using MediatR;

namespace HRMS_V2.API.Application.CommandHandler
{
    public class AddHolidayCommandHandler : IRequestHandler<AddHolidayRequest, HolidayModel>
    {
        private readonly IHolidayService _holidayService;

        public AddHolidayCommandHandler(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        public async Task<HolidayModel> Handle(AddHolidayRequest request, CancellationToken cancellationToken)
        {
            var holidayModel = await _holidayService.CreateHolidayAsync(request.Holiday);

            return holidayModel;
        }
    }
}
