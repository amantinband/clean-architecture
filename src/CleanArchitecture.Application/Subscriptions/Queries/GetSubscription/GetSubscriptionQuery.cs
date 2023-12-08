using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Subscriptions.Common;

using ErrorOr;

namespace CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

public record GetSubscriptionQuery(Guid UserId, Guid SubscriptionId)
    : IAuthorizeableRequest<ErrorOr<SubscriptionResult>>;