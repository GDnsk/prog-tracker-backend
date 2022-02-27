using ProgTracker.Domain.Entity;
using ProgTracker.Domain.Repository;
using MongoDB.Driver;

namespace ProgTracker.Infrastructure.MongoDb.Repository;

public class UserRepository : BaseReadWriteRepository<User>, IUserRepository
{
    public UserRepository(IMongoDatabase db) : base(db)
    {
    }

    public User FindByEmail(string email)
    {
        return FindOne(x => x.Email == email);
    }
}