using System.Security.Claims;
using ProgTracker.Application.Common.Interfaces;
using ProgTracker.Domain.Entity;
using ProgTracker.Domain.Model.Auth;

namespace ProgTracker.Infrastructure.AspNetCore.Security;

public class AuthManager : IAuthManager
{
    private readonly string _secret;
    private readonly string _iss;
    private readonly string _aud;
    private readonly int _expiresSeconds;
    private readonly string _membershipKey;
    
    public AuthManager(string secret, string iss, string aud, int expiresSeconds, string membershipKey)
    {
        _secret = secret;
        _iss = iss;
        _aud = aud;
        _expiresSeconds = expiresSeconds;
        _membershipKey = membershipKey;
    }

    public string CreateToken(User user)
    {
        var identity = new ClaimsIdentity(new[] {
            new Claim("ui", user.Id.Value),
            new Claim("ue", user.Email)
        });
            
        return JwtUtil.Create(
            _secret, 
            _iss, 
            _aud, 
            _expiresSeconds, 
            new ClaimsIdentity(identity));
    }
    

    public bool IsPasswordValid(AuthModel model, User user)
    {
        var membership = GetMemberShip();
        return membership.Compare(model.Password, user.Password);
    }
        
    public Membership GetMemberShip()
    {
        // hardcode por que no finanças esse valor não irá mudar
        var membership = new Membership(_membershipKey);
        return membership;
    }
    
    public string EncodePassword(string password)
    {
        var membership = GetMemberShip();

        return membership.Encode(password);
    }
}

