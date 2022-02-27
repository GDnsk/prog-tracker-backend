using ProgTracker.Domain.Entity;

namespace ProgTracker.Domain.Repository;

public interface IUserRepository : IBaseRepository<User>
{
    public User FindByEmail(string email);
}