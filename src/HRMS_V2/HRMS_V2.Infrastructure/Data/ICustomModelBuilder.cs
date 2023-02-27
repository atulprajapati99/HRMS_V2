using Microsoft.EntityFrameworkCore;

namespace HRMS_V2.Infrastructure.Data;

public interface ICustomModelBuilder
{
    void Build(ModelBuilder modelBuilder);
}