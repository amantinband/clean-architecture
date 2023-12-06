using CleanArchitecture.Application.Subscriptions.Common;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

public record GetSubscriptionQuery : IRequest<ErrorOr<SubscriptionResult>>;