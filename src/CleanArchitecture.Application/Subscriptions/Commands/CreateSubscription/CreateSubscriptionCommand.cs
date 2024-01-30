using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Policies;
using CleanArchitecture.Application.Common.Security.Request;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;

[Authorize(Permissions = Permission.Subscription.Create, Policies = Policy.SelfOrAdmin)]
public class CreateSubscriptionCommand : IAuthorizeableRequest<Result<SubscriptionResult>>
{
    private CreateSubscriptionCommand(Guid userId, string firstName, string lastName, string email, SubscriptionType subscriptionType)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        SubscriptionType = subscriptionType;
    }

    public static Result<CreateSubscriptionCommand> TryCreate(Guid userId, string firstName, string lastName, string email, string subscriptionType)
    {
        if (!SubscriptionType.TryFromName(subscriptionType, out var subscription_type))
        {
            return Result.Failure<CreateSubscriptionCommand>(Error.Validation("Invalid plan type", nameof(subscriptionType)));
        }

        return Result.Success(new CreateSubscriptionCommand(userId, firstName, lastName, email, subscription_type));
    }

    public Guid UserId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public SubscriptionType SubscriptionType { get; }
}
