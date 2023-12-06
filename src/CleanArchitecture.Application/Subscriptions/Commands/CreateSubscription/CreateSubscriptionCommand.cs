using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(SubscriptionType SubscriptionType)
    : IRequest<ErrorOr<SubscriptionResult>>;