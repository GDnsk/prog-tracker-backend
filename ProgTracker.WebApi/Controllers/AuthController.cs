using ProgTracker.Application.Common.Interfaces;
using ProgTracker.Application.Users.Register;
using ProgTracker.Domain.Entity;
using ProgTracker.Domain.Model.Auth;
using ProgTracker.Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProgTracker.WebApi.Controllers;

[AllowAnonymous]
public class AuthController : BaseApiController
{
    private readonly IAuthManager _authService;
    public AuthController(IAuthManager authService, IMediator mediator) : base(mediator) => _authService = authService;

    [HttpPost]
    [Route("login")]
    public object Login(
        [FromServices] IAuthManager authManager,
        [FromServices] IUserRepository userRepository,
        [FromBody] AuthModel login)
    {
        var user = userRepository.FindByEmail(login.Email);

        if (user == default)
        {
            return BadRequest();
        }

        if (!_authService.IsPasswordValid(login, user))
        {
            return BadRequest();
        }

        var token = authManager.CreateToken(user);
        
        return new {
            AccessToken = token,
            Email = user.Email,
            Name = user.Name,
        };
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<User>> Register([FromBody] RegisterUserInputModel inputModel)
    {
        await Send(new RegisterUserCommand(inputModel));
        return Accepted();
    }
}