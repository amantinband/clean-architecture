using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<CreateUserCommand, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.PlanType, request.FullName);

        await _usersRepository.AddUserAsync(user);

        return user;
    }
}
