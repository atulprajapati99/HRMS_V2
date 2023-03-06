using HRMS_V2.Application.Models.Base;

namespace HRMS_V2.Application.Models
{
    public class HolidayModel : BaseModel
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public string Day { get; set; }

        public string RecordStatus { get; set; }
    }
}
