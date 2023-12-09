using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Policies;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Domain.Users;

using ErrorOr;

namespace CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;

[Authorize(Permissions = Permission.Subscription.Create, Policies = Policy.SelfOrAdmin)]
public record CreateSubscriptionCommand(Guid UserId, SubscriptionType SubscriptionType)
    : IAuthorizeableRequest<ErrorOr<SubscriptionResult>>;