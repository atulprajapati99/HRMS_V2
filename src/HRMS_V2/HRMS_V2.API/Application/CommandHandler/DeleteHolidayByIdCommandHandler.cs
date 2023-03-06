using HRMS_V2.API.Requests;
using HRMS_V2.Application.Interfaces;
using MediatR;

namespace HRMS_V2.API.Application.CommandHandler
{
    public class DeleteHolidayByIdCommandHandler : IRequestHandler<DeleteHolidayByIdRequest,Unit>
    {
        private readonly IHolidayService _holidayService;

        public DeleteHolidayByIdCommandHandler(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        public async Task<Unit> Handle(DeleteHolidayByIdRequest request, CancellationToken cancellationToken)
        {
            await _holidayService.DeleteHolidayByIdAsync(request.Id);

            return Unit.Value;
        }        
    }
}
