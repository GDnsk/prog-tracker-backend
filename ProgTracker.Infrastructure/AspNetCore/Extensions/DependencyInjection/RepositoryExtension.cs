using ProgTracker.Domain.Repository;
using ProgTracker.Infrastructure.MongoDb.Repository;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ProgTracker.Infrastructure.AspNetCore.Extensions.DependencyInjection;

public static class RepositoryExtension
{
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository>(s => new UserRepository(s.GetService<IMongoDatabase>()));
        services.AddSingleton<ITraceRepository>(s => new TraceRepository(s.GetService<IMongoDatabase>()));
    }
}