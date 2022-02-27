
using ProgTracker.Domain.Model;
using ProgTracker.Domain.Model.Database;
using ProgTracker.Infrastructure.MongoDb.Serialization;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ProgTracker.Infrastructure.MongoDb
{
    public static class ConfigurationContext
    {
        public static void RegisterMongoDbMappings()
        {
            // realizar os mapeamentos
            BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));
            BsonSerializer.RegisterSerializer(typeof(Id), new IdSerializer());
            BsonSerializer.RegisterIdGenerator(typeof(Id), new IdGenerator()); 
        } 
    }
}