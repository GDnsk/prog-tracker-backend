using ProgTracker.Domain.Entity.Interfaces;
using ProgTracker.Domain.Model;
using ProgTracker.Domain.Model.Database;
using MongoDB.Driver;

namespace ProgTracker.Infrastructure.MongoDb.Repository;

public abstract class BaseReadWriteRepository<T> : BaseReadRepository<T> where T : IBaseEntity
{
    protected BaseReadWriteRepository(IMongoDatabase db) : base(db)
    {
    }

    protected BaseReadWriteRepository(IMongoCollection<T> collection) : base(collection)
    {
    }

    protected BaseReadWriteRepository(IMongoDatabase db, string collectionName) : base(db, collectionName)
    {
    }
    
    public void Replace(T entity)
    {
        if (entity.Id == default)
        {
            throw new ArgumentException("Id default is invalid to this operation");
        }

        entity.ModifiedAt = DateTime.Now;

        _collection.ReplaceOne(
            x => x.Id == entity.Id,
            entity
        );
    }
    
    public void Save(T entity)
    {
        entity.CreatedAt = DateTime.Now;
        _collection.InsertOne(entity);
    }
    
    public void Delete(Id id)
    {
        _collection.DeleteOne(x => x.Id == id);
    }

}