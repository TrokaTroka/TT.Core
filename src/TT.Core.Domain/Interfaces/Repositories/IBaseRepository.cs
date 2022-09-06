using System.Linq.Expressions;
using TT.Core.Domain.Entities;

namespace TT.Core.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : EntityBase
{
    Task<IEnumerable<T>> SearchAll(Expression<Func<T, bool>> predicate);
    Task<T> SearchOne(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetAll();
    Task<T> GetById(Guid id);
    Task<T> Create(T entity);
    Task Update(T entity);
    void Delete(T entity);
    void DeleteMany(IEnumerable<T> entities);
}
