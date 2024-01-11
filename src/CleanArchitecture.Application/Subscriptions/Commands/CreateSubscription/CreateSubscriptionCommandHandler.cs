using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler(
    IUsersRepository _usersRepository) : IRequestHandler<CreateSubscriptionCommand, ErrorOr<SubscriptionResult>>
{
    public async Task<ErrorOr<SubscriptionResult>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken) =>
        await (await _usersRepository.GetByIdAsync(request.UserId, cancellationToken)).ToErrorOr()
            .When(user => user is not null, Error.Conflict(description: "User already has an active subscription"))
            .Map(_ =>
            {
                var subscription = new Subscription(request.SubscriptionType);

                var user = new User(
                    request.UserId,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    subscription);

                return (Subscription: subscription, User: user);
            })
            .TapAsync(pair => _usersRepository.AddAsync(pair.User, cancellationToken))
            .Map(pair => SubscriptionResult.FromUser(pair.User));
}
