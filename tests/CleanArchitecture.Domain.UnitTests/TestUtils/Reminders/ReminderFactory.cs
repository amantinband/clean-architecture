using CleanArchitecture.Domain.Reminders;

namespace CleanArchitecture.Domain.UnitTests.TestUtils.Reminders;

public static class ReminderFactory
{
    public static Reminder Create(
        string text = ReminderConstants.Text,
        DateTimeOffset? dateTime = null,
        Guid? id = null)
    {
        return new Reminder(
            text,
            dateTime ?? ReminderConstants.DateTime,
            id ?? ReminderConstants.Id);
    }
}