using System.Reflection;
using HRMS_V2.Core.Entities;
using HRMS_V2.Core.Entities.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;

namespace HRMS_V2.Infrastructure.Data;

public class AdminContext : IdentityDbContext<AdminUser, AdminRole, int>
{
    public AdminContext(DbContextOptions<AdminContext> options)
        : base(options)
    {
    }

    private IDbContextTransaction _currentTransaction;
    public IDbContextTransaction GetCurrentTransaction => _currentTransaction;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var typeToRegisters = typeof(Entity).GetTypeInfo().Assembly.DefinedTypes.Select(t => t.AsType());

        modelBuilder.RegisterEntities(typeToRegisters);

        modelBuilder.RegisterConvention();

       // modelBuilder.HasDefaultSchema("public");

        base.OnModelCreating(modelBuilder);

        modelBuilder.RegisterCustomMappings(typeToRegisters);
    }

    public async Task BeginTransactionAsync()
    {
        _currentTransaction = _currentTransaction ?? await Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            _currentTransaction?.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}

static class AdminContextConfigurations
{
    internal static void RegisterEntities(this ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
    {
        var entityTypes = typeToRegisters.Where(t =>
            (t.GetTypeInfo().IsSubclassOf(typeof(Entity)) || t.GetTypeInfo().IsSubclassOf(typeof(Enumeration))) &&
            !t.GetTypeInfo().IsAbstract);

        foreach (var type in entityTypes)
        {
            modelBuilder.Entity(type);
        }
    }

    internal static void RegisterConvention(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.ClrType.Namespace != null)
            {
                var tableName = entity.ClrType.Name;
                var typeBuilder = modelBuilder.Entity(entity.Name);
                typeBuilder.ToTable(tableName);

                if (entity.ClrType.IsSubclassOf(typeof(Entity)))
                {
                }
                else if (entity.ClrType.IsSubclassOf(typeof(Enumeration)))
                {
                    typeBuilder.Property("Id").ValueGeneratedNever();
                }
            }
        }

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    internal static void RegisterCustomMappings(this ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
    {
        var customModelBuilderTypes = typeToRegisters.Where(x => typeof(ICustomModelBuilder).IsAssignableFrom(x));
        foreach (var builderType in customModelBuilderTypes)
        {
            if (builderType != null && builderType != typeof(ICustomModelBuilder))
            {
                var builder = (ICustomModelBuilder)Activator.CreateInstance(builderType);
                builder.Build(modelBuilder);
            }
        }
    }
}

public class AdminContextFactory : IDesignTimeDbContextFactory<AdminContext>
{
    public AdminContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AdminContext>();
        builder.UseSqlServer(
                "Server=ATUL-TECH\\MSSQLSERVER1;Database=HRMS_V2;User Id=sa;password=sa@123;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;")
            .UseLowerCaseNamingConvention();
        return new AdminContext(builder.Options);
    }
}