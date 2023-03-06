using System.Linq.Expressions;
using Azure.Core;
using HRMS_V2.Core.Entities;
using HRMS_V2.Core.Entities.Base;
using HRMS_V2.Core.Repositories.Base;
using HRMS_V2.Core.Specifications.Base;
using HRMS_V2.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRMS_V2.Infrastructure.Repository.Base;

public class RepositoryBase<T, TId> : IRepositoryBase<T, TId> where T : class, IEntityBase<TId>, IAuditable
{

    private readonly UserManager<AdminUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RepositoryBase(AdminContext context, UserManager<AdminUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    private readonly DbContext _context;

    private DbSet<T> _entities;

    protected virtual DbSet<T> Entities
    {
        get
        {
            if (_entities == null)
                _entities = _context.Set<T>();

            return _entities;
        }
    }

    public async virtual Task<T> GetByIdAsync(TId id)
    {
        return await Entities.FindAsync(id);
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        Entities.AddRange(entities);
        await _context.SaveChangesAsync();

        return entities;
    }

    public async Task<T> SaveAsync(T entity)
    {
        try
        {
            var userEmail = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (entity.Id == null || entity.Id.Equals(default(TId)))
            {
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = user.Id;
                entity.CreatedByName = user.FullName;                
                Entities.Add(entity);
            }
            else
            {
                entity.ModifiedDate = DateTime.Now;
                entity.ModifiedByName = user.FullName;
                entity.ModifiedBy = user.Id;
                _context.Entry(entity).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return entity;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    // TODO Implementation to update Auditable properties 
    public async Task DeleteAsync(T entity)
    {
        Entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async virtual Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await Entities.ToListAsync();
    }

    public IQueryable<T> Table => Entities;

    public IQueryable<T> TableNoTracking => Entities.AsNoTracking();

    public async Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await Table.Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeString = null,
        bool disableTracking = true)
    {
        var query = disableTracking ? TableNoTracking : Table;

        if (!string.IsNullOrWhiteSpace(includeString))
        {
            query = query.Include(includeString);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        List<Expression<Func<T, object>>> includes = null,
        bool disableTracking = true)
    {
        var query = disableTracking ? TableNoTracking : Table;

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T, TId>.GetQuery(Table, spec);
    }
}