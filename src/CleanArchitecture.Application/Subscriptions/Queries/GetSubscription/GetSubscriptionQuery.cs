using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Policies;
using CleanArchitecture.Application.Subscriptions.Common;

using ErrorOr;

namespace CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

[Authorize(Permissions = Permission.Subscription.Get, Policies = Policy.SelfOrAdmin)]
public record GetSubscriptionQuery(Guid UserId)
    : IAuthorizeableRequest<ErrorOr<SubscriptionResult>>;