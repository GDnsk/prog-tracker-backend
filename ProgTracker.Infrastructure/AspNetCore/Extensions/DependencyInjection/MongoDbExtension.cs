using ProgTracker.Infrastructure.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace ProgTracker.Infrastructure.AspNetCore.Extensions.DependencyInjection;

public static class MongoExtension
{
    public static void AddMongo(this IServiceCollection services, IConfiguration config)
    {
        ConfigurationContext.RegisterMongoDbMappings();

        services.AddSingleton<IMongoClient>(s =>
        {
            var section = config.GetSection("MongoDB");
            var connection = section.GetSection("ConnectionString").Value;

#if !DEBUG
            return new MongoClient(connection);
#else
            var url = new MongoUrlBuilder(connection);

            var client = new MongoClient(new MongoClientSettings() {
                Server = url.Server,
                ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e =>
                    {
                        Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                    });
                }
            });
            return client;
#endif
        });

        services.AddSingleton(s =>
        {
            var section = config.GetSection("MongoDB");
            var connection = section.GetSection("ConnectionString").Value;
            var url = new MongoUrlBuilder(connection);
            return s.GetService<IMongoClient>().GetDatabase(url.DatabaseName);
        });
    }
}