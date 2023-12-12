using CleanArchitecture.Contracts.Subscriptions;

namespace CleanArchitecture.Api.IntegrationTests.Common.Subscriptions;

public static class SubscriptionRequestFactory
{
    public static CreateSubscriptionRequest CreateCreateSubscriptionRequest(
        SubscriptionType? subscriptionType = null)
    {
        return new(subscriptionType ?? SubscriptionType.Basic);
    }
}