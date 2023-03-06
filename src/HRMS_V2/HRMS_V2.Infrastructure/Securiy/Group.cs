using System.ComponentModel;

namespace HRMS_V2.Infrastructure.Securiy
{
    public enum Group
    {
        [Description("Dashboard")]
        Dashboard = 1,

        [Description("Profile")]
        Profile = 2,

        [Description("Attendance")]
        Attendance = 3,

        [Description("Holiday")]
        Holiday = 4,
    }
}
