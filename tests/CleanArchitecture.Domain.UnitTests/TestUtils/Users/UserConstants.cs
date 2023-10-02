using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.UnitTests.TestUtils.Users;

public static class UserConstants
{
    public const string FullName = "Amichai Mantinband";
    public static Guid Id = Guid.NewGuid();
    public static Guid CalendarId = Guid.NewGuid();
    public static PlanType Plan = PlanType.Basic;
    public static Calendar Calendar = Calendar.Empty();
}