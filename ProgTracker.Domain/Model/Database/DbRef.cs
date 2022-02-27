using ProgTracker.Domain.Entity.Interfaces;

namespace ProgTracker.Domain.Model.Database;

public class DbRef<T> where T : IBaseEntity
{
    public Id Id { get; set; }
    public string Ref { get; set; }

    public DbRef(Id id)
    {
        Id = id;
        Ref = typeof(T).Name;
    }

    public DbRef(T entity)
    {
        Id = entity.Id;
        Ref = typeof(T).Name;
    }
}