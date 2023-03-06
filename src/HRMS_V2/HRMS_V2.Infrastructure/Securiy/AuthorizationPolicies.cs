namespace HRMS_V2.Infrastructure.Securiy
{
    public static class AuthorizationPolicies
    {
        public const string SystemAdminPolicy = "SystemAdmin";
        public const string HRPolicy = "HRAdmin";
        public const string ManagerPolicy = "MangerAdmin";
        public const string EmployeePolicy = "Employee";

        public const string ManageHolidayRequirement = "ManageHolidayRequirement";
        public const string ReadHolidayRequirement = "ReadHolidayRequirement";
      
    }
}
