using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler(
    ICurrentUserProvider _currentUserProvider,
    IUsersRepository _usersRepository) : IRequestHandler<CreateSubscriptionCommand, ErrorOr<SubscriptionResult>>
{
    public async Task<ErrorOr<SubscriptionResult>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();

        if (await _usersRepository.GetByIdAsync(currentUser.Id) is not null)
        {
            return Error.Conflict(description: "User already has an active subscription");
        }

        var subscription = new Subscription(request.SubscriptionType);

        var user = new User(
            currentUser.Id,
            currentUser.FirstName,
            currentUser.LastName,
            subscription);

        await _usersRepository.AddAsync(user);

        return SubscriptionResult.FromUser(user);
    }
}
