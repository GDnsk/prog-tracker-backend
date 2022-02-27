using ProgTracker.Domain.Entity;
using ProgTracker.Domain.Repository;
using MongoDB.Driver;

namespace ProgTracker.Infrastructure.MongoDb.Repository;

public class TraceRepository : BaseReadWriteRepository<Trace>, ITraceRepository
{
    public TraceRepository(IMongoDatabase db) : base(db)
    {
    }

}