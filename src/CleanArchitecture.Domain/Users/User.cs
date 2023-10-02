using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Users.Events;

using ErrorOr;

namespace CleanArchitecture.Domain.Users;

public class User : Entity
{
    private readonly int _maxDailyReminders;
    private readonly PlanType _plan = null!;
    private readonly string _fullName = null!;
    private readonly Calendar _calendar = null!;

    private readonly List<Guid> _reminderIds = [];

    public User(PlanType planType, string fullName, Calendar? calendar = null, Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        _maxDailyReminders = planType.GetMaxDailyReminders();
        _plan = planType;
        _fullName = fullName;
        _calendar = calendar ?? Calendar.Empty();
    }

    public ErrorOr<Success> SetReminder(Reminder reminder)
    {
        if (HasReachedDailyReminderLimit(reminder.DateTime))
        {
            return UserErrors.CannotCreateMoreRemindersThanPlanAllows;
        }

        _reminderIds.Add(reminder.Id);

        _domainEvents.Add(new ReminderSetEvent(reminder));

        return Result.Success;
    }

    private bool HasReachedDailyReminderLimit(DateTimeOffset dateTime)
    {
        var dailyReminderCount = _calendar.GetNumEventsOnDay(dateTime.Date);

        return dailyReminderCount >= _plan.GetMaxDailyReminders() || dailyReminderCount == int.MaxValue;
    }

    private User()
    {
    }
}