using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProgTracker.Infrastructure.AspNetCore.Security;

public static class JwtUtil
{

    /// <summary>
    /// Criar um token JWT
    /// </summary>
    /// <param name="secret">Token secreto de 256 bits</param>
    /// <param name="issuer">Quem é o responsável pelo token (quem gerou)</param>
    /// <param name="audience">Qual sistema pode usar esse token</param>
    /// <param name="expiresSeconds">Tempo de expiração em segundos</param>
    /// <param name="subject">Payload de informações uteis dentro do token, tipo id de usuário, id de cliente, etc</param>
    /// <returns>Token jwt em formato string</returns>
    public static string Create(
        string secret, 
        string issuer, 
        string audience, 
        int expiresSeconds,
        ClaimsIdentity subject)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.UtcNow.AddSeconds(expiresSeconds),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}