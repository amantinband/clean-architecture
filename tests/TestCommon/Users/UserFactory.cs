using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users;

using TestCommon.TestConstants;

namespace TestCommon.Users;

public static class UserFactory
{
    public static User CreateUser(
        UserId? id = null,
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string emailName = Constants.User.Email,
        Subscription? subscription = null)
    {
        return new User(
            id ?? UserId.TryCreate(Constants.User.Id).Value,
            firstName,
            lastName,
            emailName,
            subscription ?? SubscriptionFactory.CreateSubscription());
    }
}