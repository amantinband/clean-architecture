using CleanArchitecture.Application.Reminders.Commands.DeleteReminder;
using CleanArchitecture.Application.Reminders.Commands.DismissReminder;
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

    public static DismissReminderCommand CreateDismissReminderCommand(
        Guid? userId = null,
        Guid? subscriptionId = null,
        Guid? reminderId = null)
    {
        return new DismissReminderCommand(
            userId ?? Constants.User.Id,
            subscriptionId ?? Constants.Subscription.Id,
            reminderId ?? Constants.Reminder.Id);
    }

    public static DeleteReminderCommand CreateDeleteReminderCommand(
        Guid? userId = null,
        Guid? subscriptionId = null,
        Guid? reminderId = null)
    {
        return new DeleteReminderCommand(
            userId ?? Constants.User.Id,
            subscriptionId ?? Constants.Subscription.Id,
            reminderId ?? Constants.Reminder.Id);
    }
}