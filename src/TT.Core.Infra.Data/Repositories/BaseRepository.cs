using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.Data.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : EntityBase
{
    protected readonly TrokaTrokaDbContext _context;
    protected readonly DbSet<T> _contextSet;
    protected BaseRepository(TrokaTrokaDbContext context)
    {
        _context = context;
        _contextSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> SearchAll(Expression<Func<T, bool>> predicate)
    {
        return await _contextSet.AsNoTracking().Where(predicate).ToListAsync();
    }
    public async Task<T> SearchOne(Expression<Func<T, bool>> predicate)
    {
        return await _contextSet.AsNoTracking().Where(predicate).FirstOrDefaultAsync();
    }

    public virtual async Task<List<T>> GetAll()
    {
        return await _contextSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<T> GetById(Guid id)
    {
        return await _contextSet.FindAsync(id);
    }

    public virtual async Task<T> Create(T entity)
    {
        await _contextSet.AddAsync(entity);
        _context.SaveChanges();

        return entity;
    }

    public virtual async Task Update(T entity)
    {
        _contextSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual void Delete(T entity)
    {
        _contextSet.Remove(entity);
        _context.SaveChanges();
    }

    public void DeleteMany(IEnumerable<T> entities)
    {
        _contextSet.RemoveRange(entities);
        _context.SaveChanges();
    }
}
