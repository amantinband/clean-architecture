using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<CancelSubscriptionCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken) =>
        await (await _usersRepository.GetByIdAsync(request.UserId, cancellationToken)).ToErrorOr()
            .FailIf(user => user is null, Error.NotFound(description: "User not found"))
            .Then(user => user!.CancelSubscription(request.SubscriptionId).Then(success => user!))
            .ThenDoAsync(user => _usersRepository.UpdateAsync(user, cancellationToken))
            .Then(_ => Result.Success);
}
