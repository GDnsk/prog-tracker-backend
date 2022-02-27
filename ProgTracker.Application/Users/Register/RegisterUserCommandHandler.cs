using ProgTracker.Application.Common;
using ProgTracker.Application.Common.Interfaces;
using ProgTracker.Domain.Entity;
using ProgTracker.Domain.Repository;
using MediatR;

namespace ProgTracker.Application.Users.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<User>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthManager _authManager;

    public RegisterUserCommandHandler(IUserRepository userRepository, IAuthManager authManager)
    {
        _userRepository = userRepository;
        _authManager = authManager;
    }

    public Task<Response<User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var input = request.InputModel;
        var user = new User()
        {
            Email = input.Email,
            Name = input.Name,
            Password = _authManager.EncodePassword(input.Password)
        };

        _userRepository.Save(user);

        return Task.FromResult(new Response<User>(user));
    }
}