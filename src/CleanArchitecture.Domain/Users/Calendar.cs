namespace CleanArchitecture.Domain.Users;

public class Calendar
{
    /// <summary>
    /// day -> num events.
    /// </summary>
    private readonly Dictionary<DateOnly, int> _calendar = [];

    public static Calendar Empty()
    {
        return new Calendar();
    }

    public void IncrementEventCount(DateOnly date)
    {
        if (!_calendar.ContainsKey(date))
        {
            _calendar[date] = 0;
        }

        _calendar[date]++;
    }

    public void DecrementEventCount(DateOnly date)
    {
        if (!_calendar.ContainsKey(date))
        {
            return;
        }

        _calendar[date]--;
    }

    public void SetEventCount(DateOnly date, int numEvents)
    {
        _calendar[date] = numEvents;
    }

    public int GetNumEventsOnDay(DateTimeOffset dateTime)
    {
        return _calendar.TryGetValue(DateOnly.FromDateTime(dateTime.Date), out var numEvents)
            ? numEvents
            : 0;
    }

    private Calendar()
    {
    }
}