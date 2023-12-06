using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users.Events;

using ErrorOr;

namespace CleanArchitecture.Domain.Users;

public class User : Entity
{
    private readonly Calendar _calendar = null!;
    private readonly List<Guid> _reminderIds = [];

    public string FirstName { get; } = null!;
    public string LastName { get; } = null!;

    public Subscription Subscription { get; } = null!;

    public User(
        Guid id,
        string firstName,
        string lastName,
        Subscription subscription,
        Calendar? calendar = null)
            : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Subscription = subscription;
        _calendar = calendar ?? Calendar.Empty();
    }

    public ErrorOr<Success> SetReminder(Reminder reminder)
    {
        if (HasReachedDailyReminderLimit(reminder.DateTime))
        {
            return UserErrors.CannotCreateMoreRemindersThanPlanAllows;
        }

        _calendar.IncrementEventCount(DateOnly.FromDateTime(reminder.DateTime.Date));

        _reminderIds.Add(reminder.Id);

        _domainEvents.Add(new ReminderSetEvent(reminder));

        return Result.Success;
    }

    private bool HasReachedDailyReminderLimit(DateTimeOffset dateTime)
    {
        var dailyReminderCount = _calendar.GetNumEventsOnDay(dateTime.Date);

        return dailyReminderCount >= Subscription.SubscriptionType.GetMaxDailyReminders() || dailyReminderCount == int.MaxValue;
    }

    private User()
    {
    }
}