using System.Security.Claims;
using ProgTracker.Domain.Entity;
using ProgTracker.Domain.Model.Database;
using Microsoft.AspNetCore.Authentication;

namespace ProgTracker.WebApi.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context)
    {
        var aud = context.User.FindFirstValue("aud");
        if (aud == "User")
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var user = new User { Id = new Id(context.User.FindFirstValue("ui")) };
            if (user.Id == default)
            {
                await context.ForbidAsync();
                return;
            }
        
            user.Email = context.User.FindFirstValue("ue");
            context.Items.Add("User", user);
            await _next(context);
        }
        
        await _next(context);
    }
}