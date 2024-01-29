using CleanArchitecture.Application.Common.Security.Request;
using CleanArchitecture.Application.Common.Security.Roles;

namespace CleanArchitecture.Application.Subscriptions.Commands.CancelSubscription;

[Authorize(Roles = Role.Admin)]
public record CancelSubscriptionCommand(Guid UserId, Guid SubscriptionId) : IAuthorizeableRequest<Result<Unit>>;