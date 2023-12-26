using CleanArchitecture.Domain.Reminders;

using TestCommon.TestConstants;

namespace TestCommon.Reminders;

public static class ReminderFactory
{
    public static Reminder CreateReminder(
        Guid? userId = null,
        Guid? subscriptionId = null,
        string text = Constants.Reminder.Text,
        DateTime? dateTime = null,
        Guid? id = null)
    {
        return new Reminder(
            userId ?? Constants.User.Id,
            subscriptionId ?? Constants.Subscription.Id,
            text ?? Constants.Reminder.Text,
            dateTime ?? Constants.Reminder.DateTime,
            id ?? Constants.Reminder.Id);
    }
}