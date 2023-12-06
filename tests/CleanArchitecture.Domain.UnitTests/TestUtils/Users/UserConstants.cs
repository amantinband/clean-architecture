using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.UnitTests.TestUtils.Users;

public static class UserConstants
{
    public const string FirstName = "Amichai";
    public const string LastName = "Mantinband";
    public static Guid Id = Guid.NewGuid();
    public static Guid CalendarId = Guid.NewGuid();
    public static SubscriptionType SubscriptionType = SubscriptionType.Basic;
    public static Calendar Calendar = Calendar.Empty();
}