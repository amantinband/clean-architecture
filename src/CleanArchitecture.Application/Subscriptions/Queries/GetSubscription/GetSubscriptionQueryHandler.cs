using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Domain.Users;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

public class GetSubscriptionQueryHandler(IUsersRepository _usersRepository)
    : IRequestHandler<GetSubscriptionQuery, Result<SubscriptionResult>>
{
    public async Task<Result<SubscriptionResult>> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
        => await UserId.TryCreate(request.UserId)
            .BindAsync(userId => _usersRepository.GetByIdAsync(userId, cancellationToken).ToResultAsync(Error.NotFound("Subscription not found.")))
            .MapAsync(SubscriptionResult.FromUser);
}
