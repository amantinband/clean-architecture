using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Domain.Users;

using ErrorOr;

namespace CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(Guid UserId, SubscriptionType SubscriptionType)
    : IAuthorizeableRequest<ErrorOr<SubscriptionResult>>;