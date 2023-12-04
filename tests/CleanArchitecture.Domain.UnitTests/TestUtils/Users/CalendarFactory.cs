using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.UnitTests.TestUtils.Users;

public static class CalendarFactory
{
    public static Calendar Create(DateOnly date, int numEvents, Guid? id = null)
    {
        var calendar = Calendar.Empty();

        calendar.SetEventCount(date, numEvents);

        return calendar;
    }
}