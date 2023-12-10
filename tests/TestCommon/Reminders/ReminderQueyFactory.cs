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
}