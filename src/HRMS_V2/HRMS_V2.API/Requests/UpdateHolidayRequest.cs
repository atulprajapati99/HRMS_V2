using HRMS_V2.Application.Models;
using MediatR;

namespace HRMS_V2.API.Requests
{
    public class UpdateHolidayRequest : IRequest<Unit>
    {
        public HolidayModel Holiday { get; set; }
    }
}
