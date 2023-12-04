using CleanArchitecture.Domain.Users;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Users.Commands.CreateUser;

public record CreateUserCommand(PlanType PlanType, string FullName) : IRequest<ErrorOr<User>>;