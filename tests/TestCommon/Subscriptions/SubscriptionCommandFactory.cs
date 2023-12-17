using CleanArchitecture.Application.Subscriptions.Commands.CancelSubscription;
using CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;
using CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;
using CleanArchitecture.Domain.Users;

using TestCommon.TestConstants;

namespace TestCommon.Subscriptions;

public static class SubscriptionCommandFactory
{
    public static CreateSubscriptionCommand CreateCreateSubscriptionCommand(
        Guid? userId = null,
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string emailName = Constants.User.Email,
        SubscriptionType? subscriptionType = null)
    {
        return new CreateSubscriptionCommand(
            userId ?? Constants.User.Id,
            firstName,
            lastName,
            emailName,
            subscriptionType ?? Constants.Subscription.Type);
    }

    public static CancelSubscriptionCommand CreateCancelSubscriptionCommand(
        Guid? userId = null,
        Guid? subscriptionId = null)
    {
        return new CancelSubscriptionCommand(
            userId ?? Constants.User.Id,
            subscriptionId ?? Constants.Subscription.Id);
    }
}