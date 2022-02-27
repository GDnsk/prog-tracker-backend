using System.Linq.Expressions;
using ProgTracker.Domain.Entity.Interfaces;
using ProgTracker.Domain.Model;
using ProgTracker.Domain.Model.Database;

namespace ProgTracker.Domain.Repository;

public interface IBaseRepository<T> where T : IBaseEntity
{
    public void Save(T entity);
    public void Replace(T entity);
    public void Delete(Id id);
    public T Get(Id id);
    public IEnumerable<T> FindAll(Expression<Func<T, bool>> filter);
}