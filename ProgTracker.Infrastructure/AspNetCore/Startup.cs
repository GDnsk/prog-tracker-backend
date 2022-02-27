using ProgTracker.Application.Common.Interfaces;
using ProgTracker.Infrastructure.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace ProgTracker.Infrastructure.AspNetCore;

public static class Startup
{
    public static void Init(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongo(configuration);
        services.AddJwtSecurity(configuration);
        services.AddJwtAuthorizationSecurity(configuration);
        services.AddRepository();
        
        services.AddMediatR(typeof(IAuthManager));

    }
}