using CleanArchitecture.Contracts.Subscriptions;

namespace CleanArchitecture.Api.IntegrationTests.Common.Subscriptions;

public static class SubscriptionRequestFactory
{
    public static CreateSubscriptionRequest CreateCreateSubscriptionRequest(
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string emailName = Constants.User.Email,
        SubscriptionType? subscriptionType = null)
    {
        return new(
            firstName,
            lastName,
            emailName,
            subscriptionType ?? SubscriptionType.Basic);
    }
}