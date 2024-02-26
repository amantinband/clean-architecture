using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Users;

using TestCommon.TestConstants;

namespace TestCommon.Reminders;

public static class ReminderFactory
{
    public static Reminder CreateReminder(
        UserId? userId = null,
        Guid? subscriptionId = null,
        string text = Constants.Reminder.Text,
        DateTime? dateTime = null,
        ReminderId? id = null)
    {
        return new Reminder(
            userId ?? UserId.TryCreate(Constants.User.Id).Value,
            subscriptionId ?? Constants.Subscription.Id,
            text ?? Constants.Reminder.Text,
            dateTime ?? Constants.Reminder.DateTime,
            id ?? ReminderId.TryCreate(Constants.Reminder.Id).Value);
    }
}