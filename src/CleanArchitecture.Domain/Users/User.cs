using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users.Events;

using Throw;

namespace CleanArchitecture.Domain.Users;

public class User : Entity
{
    private readonly Calendar _calendar = null!;

    private readonly List<Guid> _reminderIds = [];

    private readonly List<Guid> _dismissedReminderIds = [];

    public Subscription Subscription { get; private set; } = null!;

    public string Email { get; } = null!;

    public string FirstName { get; } = null!;

    public string LastName { get; } = null!;

    public User(
        Guid id,
        string firstName,
        string lastName,
        string email,
        Subscription subscription,
        Calendar? calendar = null)
            : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Subscription = subscription;
        _calendar = calendar ?? Calendar.Empty();
    }

    public Result<Unit> SetReminder(Reminder reminder)
    {
        if (Subscription == Subscription.Canceled)
        {
            return Error.NotFound("Subscription not found");
        }

        reminder.SubscriptionId.Throw().IfNotEquals(Subscription.Id);

        if (HasReachedDailyReminderLimit(reminder.DateTime))
        {
            return UserErrors.CannotCreateMoreRemindersThanSubscriptionAllows;
        }

        _calendar.IncrementEventCount(reminder.Date);

        _reminderIds.Add(reminder.Id);

        _domainEvents.Add(new ReminderSetEvent(reminder));

        return Result.Success();
    }

    public Result<Unit> DismissReminder(Guid reminderId)
    {
        if (Subscription == Subscription.Canceled)
        {
            return Error.NotFound("Subscription not found");
        }

        if (!_reminderIds.Contains(reminderId))
        {
            return Error.NotFound("Reminder not found");
        }

        if (_dismissedReminderIds.Contains(reminderId))
        {
            return Error.Conflict("Reminder already dismissed");
        }

        _dismissedReminderIds.Add(reminderId);

        _domainEvents.Add(new ReminderDismissedEvent(reminderId));

        return Result.Success();
    }

    public Result<Unit> CancelSubscription(Guid subscriptionId)
    {
        if (subscriptionId != Subscription.Id)
        {
            return Error.NotFound("Subscription not found");
        }

        Subscription = Subscription.Canceled;

        _domainEvents.Add(new SubscriptionCanceledEvent(this, subscriptionId));

        return Result.Success();
    }

    public Result<Unit> DeleteReminder(Reminder reminder)
    {
        if (Subscription == Subscription.Canceled)
        {
            return Error.NotFound("Subscription not found");
        }

        if (!_reminderIds.Remove(reminder.Id))
        {
            return Error.NotFound("Reminder not found", target: reminder.Id.ToString());
        }

        _dismissedReminderIds.Remove(reminder.Id);

        _calendar.DecrementEventCount(reminder.Date);

        _domainEvents.Add(new ReminderDeletedEvent(reminder.Id));

        return Result.Success();
    }

    public void DeleteAllReminders()
    {
        _reminderIds.ForEach(reminderId => _domainEvents.Add(new ReminderDeletedEvent(reminderId)));

        _reminderIds.Clear();
    }

    private bool HasReachedDailyReminderLimit(DateTimeOffset dateTime)
    {
        var dailyReminderCount = _calendar.GetNumEventsOnDay(dateTime.Date);

        return dailyReminderCount >= Subscription.SubscriptionType.GetMaxDailyReminders()
            || dailyReminderCount == int.MaxValue;
    }

    private User()
    {
    }
}