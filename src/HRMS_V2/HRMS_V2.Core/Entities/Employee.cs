using HRMS_V2.Core.Entities.Base;

namespace HRMS_V2.Core.Entities;

public class Employee : Entity
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}