using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users;

using TestCommon.TestConstants;

namespace TestCommon.Users;

public static class SubscriptionFactory
{
    public static Subscription CreateSubscription(
        SubscriptionType? subscriptionType = null,
        Guid? id = null)
    {
        return new Subscription(
            subscriptionType ?? Constants.Subscription.Type,
            id ?? Constants.Subscription.Id);
    }
}