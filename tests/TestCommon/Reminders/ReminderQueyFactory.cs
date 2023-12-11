using CleanArchitecture.Application.Reminders.Queries.GetReminder;
using CleanArchitecture.Application.Reminders.Queries.ListReminders;

using TestCommon.TestConstants;

namespace TestCommon.Reminders;

public static class ReminderQueryFactory
{
    public static ListRemindersQuery CreateListRemindersQuery(
        Guid? userId = null,
        Guid? subscriptionId = null)
    {
        return new ListRemindersQuery(
            userId ?? Constants.User.Id,
            subscriptionId ?? Constants.Subscription.Id);
    }

    public static GetReminderQuery CreateGetReminderQuery(
        Guid? userId = null,
        Guid? subscriptionId = null,
        Guid? reminderId = null)
    {
        return new GetReminderQuery(
            userId ?? Constants.User.Id,
            subscriptionId ?? Constants.Subscription.Id,
            reminderId ?? Constants.Reminder.Id);
    }
}