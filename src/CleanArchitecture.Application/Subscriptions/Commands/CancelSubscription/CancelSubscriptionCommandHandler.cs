using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<CancelSubscriptionCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken) =>
        await (await _usersRepository.GetByIdAsync(request.UserId, cancellationToken)).ToErrorOr()
            .When(user => user is null, Error.NotFound(description: "User not found"))
            .Map(user => user!.CancelSubscription(request.SubscriptionId).Map(success => user!))
            .TapAsync(user => _usersRepository.UpdateAsync(user, cancellationToken))
            .Map(_ => Result.Success);
}
