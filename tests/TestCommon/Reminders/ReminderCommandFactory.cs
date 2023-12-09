using CleanArchitecture.Application.Reminders.Commands.SetReminder;

using TestCommon.TestConstants;

namespace TestCommon.Reminders;

public static class ReminderCommandFactory
{
    public static SetReminderCommand CreateSetReminderCommand(
        Guid? userId = null,
        Guid? subscriptionId = null,
        string text = Constants.Reminder.Text,
        DateTime? dateTime = null)
    {
        return new SetReminderCommand(
            userId ?? Constants.User.Id,
            subscriptionId ?? Constants.Subscription.Id,
            text,
            dateTime ?? Constants.Reminder.DateTime);
    }
}