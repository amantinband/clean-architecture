using CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

using TestCommon.TestConstants;

namespace TestCommon.Subscriptions;

public static class SubscriptionQueryFactory
{
    public static GetSubscriptionQuery CreateGetSubscriptionQuery(
        Guid? userId = null)
    {
        return new GetSubscriptionQuery(userId ?? Constants.User.Id);
    }
}