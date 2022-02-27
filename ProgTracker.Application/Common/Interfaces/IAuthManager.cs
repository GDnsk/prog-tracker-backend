using ProgTracker.Domain.Entity;
using ProgTracker.Domain.Model.Auth;

namespace ProgTracker.Application.Common.Interfaces;

public interface IAuthManager
{
    string CreateToken(User user);
    public bool IsPasswordValid(AuthModel model, User user);
    public string EncodePassword(string password);
}
