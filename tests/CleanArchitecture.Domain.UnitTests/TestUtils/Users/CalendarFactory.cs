using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.UnitTests.TestUtils.Users;

public static class CalendarFactory
{
    public static Calendar Create(DateOnly date, int numEvents, Guid? id = null)
    {
        var calendar = new Calendar(id ?? UserConstants.CalendarId);

        calendar.SetEventCount(date, numEvents);

        return calendar;
    }
}