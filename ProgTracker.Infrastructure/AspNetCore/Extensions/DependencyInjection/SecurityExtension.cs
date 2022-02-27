using System.Text;
using ProgTracker.Application.Common.Interfaces;
using ProgTracker.Infrastructure.AspNetCore.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ProgTracker.Infrastructure.AspNetCore.Extensions.DependencyInjection;

public static class SecurityExtension
{
    public static void AddJwtSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("Auth");
        services.AddSingleton<IAuthManager>(x => new AuthManager(
            config["Secret"], 
            config["Iss"], 
            config["Aud"], 
            int.Parse(config["ExpiresSeconds"]),
            config["membershipKey"]
            ));
    }
    
    public static void AddJwtAuthorizationSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection("Auth");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, OptionsJwtSecurity(jwtConfig));
            
        services.AddAuthorization(options =>
        {
            options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        });
    }
    
    private static Action<JwtBearerOptions> OptionsJwtSecurity(IConfigurationSection config)
    {
        return x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Secret"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = config["Iss"],
                ValidAudience = config["Aud"],
                RequireExpirationTime = true,

                // Tempo de tolerância para a expiração de um token
                // imediata
                ClockSkew = TimeSpan.Zero
            };
        };
    }
}