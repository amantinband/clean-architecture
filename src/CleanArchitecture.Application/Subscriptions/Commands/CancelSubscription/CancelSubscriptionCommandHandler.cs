using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<CancelSubscriptionCommand, Result<FunctionalDdd.Unit>>
{
    public async Task<Result<FunctionalDdd.Unit>> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken) =>
        await UserId.TryCreate(request.UserId)
        .BindAsync(userId => _usersRepository.GetByIdAsync(userId, cancellationToken).ToResultAsync(Error.NotFound("User not found")))
        .BindAsync(user => user.CancelSubscription(request.SubscriptionId).Map(r => user))
        .BindAsync(user =>
        {
            _usersRepository.UpdateAsync(user, cancellationToken);
            return Result.Success();
        });
}
