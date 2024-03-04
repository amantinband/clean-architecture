using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users;

using MediatR;

using Unit = FunctionalDdd.Unit;

namespace CleanArchitecture.Application.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<CancelSubscriptionCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken) =>
        await UserId.TryCreate(request.UserId)
        .BindAsync(userId => _usersRepository.GetByIdAsync(userId, cancellationToken).ToResultAsync(Error.NotFound("User not found")))
        .BindAsync(user => user.CancelSubscription(request.SubscriptionId)
            .TapAsync(_ => _usersRepository.UpdateAsync(user, cancellationToken)));
}
