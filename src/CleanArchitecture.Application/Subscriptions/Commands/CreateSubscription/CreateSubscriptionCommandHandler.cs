using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler(
    IUsersRepository _usersRepository) : IRequestHandler<CreateSubscriptionCommand, Result<SubscriptionResult>>
{
    public async Task<Result<SubscriptionResult>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var rUserId = UserId.TryCreate(request.UserId);
        if (rUserId.IsFailure)
        {
            return Result.Failure<SubscriptionResult>(rUserId.Error);
        }

        if (await _usersRepository.GetByIdAsync(rUserId.Value, cancellationToken) is not null)
        {
            return Error.Conflict("User already has an active subscription");
        }

        var subscription = new Subscription(request.SubscriptionType);

        var user = new User(
            rUserId.Value,
            request.FirstName,
            request.LastName,
            request.Email,
            subscription);

        await _usersRepository.AddAsync(user, cancellationToken);

        return SubscriptionResult.FromUser(user);
    }
}
