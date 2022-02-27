using ProgTracker.Application.Common;
using ProgTracker.Domain.Entity;
using MediatR;

namespace ProgTracker.Application.Users.Register;

public class RegisterUserCommand : IRequest<Response<User>>
{
    public RegisterUserCommand(RegisterUserInputModel inputModel)
    {
        InputModel = inputModel;
    }

    public RegisterUserInputModel InputModel { get; }
}