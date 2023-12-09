using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users;

using TestCommon.TestConstants;

namespace TestCommon.Users;

public static class UserFactory
{
    public static User CreateUser(
        Guid? id = null,
        Subscription? subscription = null)
    {
        return new User(
            id ?? Constants.User.Id,
            subscription ?? SubscriptionFactory.CreateSubscription());
    }
}