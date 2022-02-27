using ProgTracker.Domain.Model;
using ProgTracker.Domain.Model.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ProgTracker.Infrastructure.MongoDb.Serialization;

public class IdGenerator : IIdGenerator
{
    private static IdGenerator __instance = new IdGenerator();

    public IdGenerator()
    {
    }

    public static IdGenerator Instance 
    {
        get { return __instance; }
    }

    public object GenerateId(object container, object document)
    {
        return new Id(ObjectId.GenerateNewId().ToString());
    }

    public bool IsEmpty(object id)
    {
        return id == null || ((Id)id).Value == default;
    }
}