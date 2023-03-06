using HRMS_V2.API.Requests;
using HRMS_V2.Application.Interfaces;
using MediatR;

namespace HRMS_V2.API.Application.CommandHandler
{
    public class UpdateHolidayCommandHandler : IRequestHandler<UpdateHolidayRequest, Unit>
    {
        private readonly IHolidayService _holidayService;

        public UpdateHolidayCommandHandler(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        public async Task<Unit> Handle(UpdateHolidayRequest request, CancellationToken cancellationToken)
        {
            await _holidayService.UpdateHolidayAsync(request.Holiday);

            return Unit.Value;
        }
    }
}
