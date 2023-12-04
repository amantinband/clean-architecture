using CleanArchitecture.Domain.Users;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Users.Queries.GetUser;

public record GetUserQuery(Guid UserId) : IRequest<ErrorOr<User>>;