using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Policies;
using CleanArchitecture.Application.Common.Security.Request;
using CleanArchitecture.Application.Subscriptions.Common;

namespace CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

[Authorize(Permissions = Permission.Subscription.Get, Policies = Policy.SelfOrAdmin)]
public record GetSubscriptionQuery(Guid UserId)
    : IAuthorizeableRequest<Result<SubscriptionResult>>;