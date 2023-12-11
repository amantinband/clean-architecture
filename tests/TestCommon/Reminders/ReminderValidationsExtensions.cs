using CleanArchitecture.Application.Reminders.Commands.SetReminder;
using CleanArchitecture.Domain.Reminders;

using FluentAssertions;

namespace TestCommon.Reminders;

public static class ReminderValidator
{
    public static void AssertCreatedFrom(this Reminder reminder, SetReminderCommand command)
    {
        reminder.SubscriptionId.Should().Be(command.SubscriptionId);
        reminder.DateTime.Should().Be(command.DateTime);
        reminder.Text.Should().Be(command.Text);
        reminder.IsDismissed.Should().BeFalse();
    }
}