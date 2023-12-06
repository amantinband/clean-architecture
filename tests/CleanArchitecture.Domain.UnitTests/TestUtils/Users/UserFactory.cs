using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.UnitTests.TestUtils.Users;

public static class UserFactory
{
    public static User Create(
        SubscriptionType? SubscriptionType = null,
        string firstName = UserConstants.FirstName,
        string lastName = UserConstants.LastName,
        Calendar? calendar = null,
        Guid? id = null)
    {
        return default!;
    }
}