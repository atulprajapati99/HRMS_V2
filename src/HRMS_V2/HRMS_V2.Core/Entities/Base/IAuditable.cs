namespace HRMS_V2.Core.Entities.Base;

public interface IAuditableDate
{
    DateTime CreatedDate { get; set; }
    DateTime? ModifiedDate { get; set; }
}

public interface IAuditableUser
{
    int? CreatedBy { get; set; }
    int? ModifiedBy { get; set; }
}

public interface IAuditable
{
    DateTime CreatedDate { get; set; }
    int? CreatedBy { get; set; }
    string? CreatedByName { get; set; }
    DateTime? ModifiedDate { get; set; }
    int? ModifiedBy { get; set; }
    string? ModifiedByName { get; set; }
}

public interface IRecordStatus
{
    string RecordStatus { get; set; }
}