using MediatR;

namespace HRMS_V2.API.Requests
{
    public class DeleteHolidayByIdRequest : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
