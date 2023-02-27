namespace HRMS_V2.Core.Entities.Base;

public interface IEntityBase<TId>
{
    TId Id { get; }
}