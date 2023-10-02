using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.UnitTests.TestUtils.Users;

public static class UserFactory
{
    public static User Create(
        PlanType? planType = null,
        string fullName = UserConstants.FullName,
        Calendar? calendar = null,
        Guid? id = null)
    {
        return new User(
            planType ?? UserConstants.Plan,
            fullName,
            calendar ?? UserConstants.Calendar,
            id ?? UserConstants.Id);
    }
}