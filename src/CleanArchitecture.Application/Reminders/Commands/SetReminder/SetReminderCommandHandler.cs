using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.SetReminder;

public class SetReminderCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<SetReminderCommand, ErrorOr<Reminder>>
{
    public async Task<ErrorOr<Reminder>> Handle(SetReminderCommand command, CancellationToken cancellationToken)
    {
        var reminder = new Reminder(
            command.UserId,
            command.SubscriptionId,
            command.Text,
            command.DateTime);

        var user = await _usersRepository.GetBySubscriptionIdAsync(command.SubscriptionId, cancellationToken);

        if (user is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        var setReminderResult = user.SetReminder(reminder);

        if (setReminderResult.IsError)
        {
            return setReminderResult.Errors;
        }

        await _usersRepository.UpdateAsync(user, cancellationToken);

        return reminder;
    }
}