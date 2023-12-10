using CleanArchitecture.Application.Reminders.Commands.SetReminder;
using CleanArchitecture.Application.Reminders.Queries.ListReminders;
using CleanArchitecture.Domain.Reminders;

using ErrorOr;

using FluentAssertions;

using MediatR;

namespace TestCommon.Reminders;

public static class MediatorExtensions
{
    public static async Task<Reminder> SetReminder(
        this IMediator mediator,
        SetReminderCommand? command = null)
    {
        var result = await mediator.Send(command ?? ReminderCommandFactory.CreateSetReminderCommand());

        result.IsError.Should().BeFalse();

        return result.Value;
    }

    public static async Task<ErrorOr<List<Reminder>>> ListReminders(
        this IMediator mediator,
        ListRemindersQuery? query = null)
    {
        return await mediator.Send(query ?? ReminderQueryFactory.CreateListRemindersQuery());
    }
}
