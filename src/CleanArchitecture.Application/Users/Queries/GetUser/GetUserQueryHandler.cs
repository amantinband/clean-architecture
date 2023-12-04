using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Users.Queries.GetUser;

public class GetUserQueryHandler(IUsersRepository _usersRepository) : IRequestHandler<GetUserQuery, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await _usersRepository.GetByIdAsync(request.UserId) is User user
            ? user
            : Error.NotFound();
    }
}
