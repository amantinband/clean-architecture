using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Subscriptions.Common;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

public class GetSubscriptionQueryHandler(
    IUsersRepository _usersRepository) : IRequestHandler<GetSubscriptionQuery, ErrorOr<SubscriptionResult>>
{
    public async Task<ErrorOr<SubscriptionResult>> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user?.Subscription.Id != request.SubscriptionId)
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        return SubscriptionResult.FromUser(user);
    }
}
