using System.ComponentModel;

namespace HRMS_V2.Infrastructure.Securiy
{
    public enum Permission
    {
        [PermissionDetails(Group.Dashboard, DashboardOverview, 1, Feature.Dashboard)]
        [Description("Overview")]
        DashboardOverview = 0,

        [PermissionDetails(Group.Profile, Profile, 1, Feature.Profile)]
        [Description("Profile")]
        Profile = 1,

        [PermissionDetails(Group.Attendance, Attendance, 1, Feature.Attendance)]
        [Description("Attendance")]
        Attendance = 2,

        [PermissionDetails(Group.Holiday, Holiday, 1, Feature.Holiday)]
        [Description("Holiday")]
        Holiday = 3,
    }
}
