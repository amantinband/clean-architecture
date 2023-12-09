using CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;
using CleanArchitecture.Domain.Users;

using TestCommon.TestConstants;

namespace TestCommon.Subscriptions;

public static class SubscriptionCommandFactory
{
    public static CreateSubscriptionCommand CreateCreateSubscriptionCommand(
        Guid? userId = null,
        SubscriptionType? subscriptionType = null)
    {
        return new CreateSubscriptionCommand(
            userId ?? Constants.User.Id,
            subscriptionType ?? Constants.Subscription.Type);
    }
}