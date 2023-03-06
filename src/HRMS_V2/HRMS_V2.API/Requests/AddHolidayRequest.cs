using HRMS_V2.Application.Models;
using MediatR;

namespace HRMS_V2.API.Requests
{
    public class AddHolidayRequest : IRequest<HolidayModel>
    {
        public HolidayModel Holiday { get; set; }
    }
}
