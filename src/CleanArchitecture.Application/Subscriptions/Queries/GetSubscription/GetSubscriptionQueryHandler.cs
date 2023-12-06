using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Subscriptions.Common;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

public class GetSubscriptionQueryHandler(
    ICurrentUserProvider _currentUserProvider,
    IUsersRepository _usersRepository) : IRequestHandler<GetSubscriptionQuery, ErrorOr<SubscriptionResult>>
{
    public async Task<ErrorOr<SubscriptionResult>> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();

        var user = await _usersRepository.GetByIdAsync(currentUser.Id);

        if (user is null)
        {
            return Error.NotFound(description: "Active subscription not found.");
        }

        return SubscriptionResult.FromUser(user);
    }
}
