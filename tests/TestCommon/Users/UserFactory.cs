using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users;

using TestCommon.TestConstants;

namespace TestCommon.Users;

public static class UserFactory
{
    public static User CreateUser(
        Guid? id = null,
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string emailName = Constants.User.Email,
        Subscription? subscription = null)
    {
        return new User(
            id ?? Constants.User.Id,
            firstName,
            lastName,
            emailName,
            subscription ?? SubscriptionFactory.CreateSubscription());
    }
}