using System.Linq.Expressions;
using ProgTracker.Domain.Entity.Interfaces;
using ProgTracker.Domain.Model;
using ProgTracker.Domain.Model.Database;
using ProgTracker.Domain.Repository;
using MongoDB.Driver;

namespace ProgTracker.Infrastructure.MongoDb.Repository;

public abstract class BaseReadRepository<T> where T : IBaseEntity
{
    protected IMongoCollection<T> _collection;


    protected BaseReadRepository(IMongoDatabase db): this(db, typeof(T).Name)
    {
            
    }
    
    protected BaseReadRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }
    
    protected BaseReadRepository(IMongoDatabase db, string collectionName)
    {
        var collection = db.GetCollection<T>(collectionName);
        _collection = collection.WithReadPreference(ReadPreference.SecondaryPreferred);
    }

    protected ProjectionDefinitionBuilder<T> Projection => Builders<T>.Projection;
    protected FilterDefinitionBuilder<T> Filter => Builders<T>.Filter;
    
    public T Get(Id id)
    {
        return _collection.Find(x => x.Id == id)
            .Limit(1)
            .FirstOrDefault();
    }
    
    public IEnumerable<T> Get(Id[] id)
    {
        var filter = Filter.In(x => x.Id, id);
        return Find(filter).ToEnumerable();
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> filter)
    {
        return Find(filter).ToEnumerable();
    }
    
    protected IFindFluent<T, T> FindWithCollation(Expression<Func<T, bool>> filter)
    {
        return _collection.Find(filter, CreateFindOptionsForCollation());
    }

    protected IFindFluent<T, T> FindWithCollation(FilterDefinition<T> filter)
    {
        return _collection.Find(filter, CreateFindOptionsForCollation());
    }
    
    protected IFindFluent<T, T> Find(FilterDefinition<T> filter)
    {
        return _collection.Find(filter);
    }

    protected IFindFluent<T, T> Find(Expression<Func<T, bool>> filter)
    {
        return _collection.Find(filter);
    }
        
    protected T FindOne(Expression<Func<T, bool>> filter)
    {
        return Find(filter).Limit(1).FirstOrDefault();
    }

    protected T FindOne(FilterDefinition<T> filter)
    {
        return Find(filter).Limit(1).FirstOrDefault();
    }
    
    protected static FindOptions CreateFindOptionsForCollation()
    {
        return new FindOptions()
        {
            Collation = new Collation("pt", strength: CollationStrength.Primary, alternate: CollationAlternate.Shifted)
        };
    }
    
    protected string FilterToMongoRaw(FilterDefinition<T> filter)
    {
        return filter.Render(_collection.DocumentSerializer, _collection.Settings.SerializerRegistry).ToString();
    }
}