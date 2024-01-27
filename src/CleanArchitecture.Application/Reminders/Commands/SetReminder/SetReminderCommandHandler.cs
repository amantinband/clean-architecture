using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.SetReminder;

public class SetReminderCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<SetReminderCommand, Result<Reminder>>
{
    public async Task<Result<Reminder>> Handle(SetReminderCommand command, CancellationToken cancellationToken)
    {
        var reminder = new Reminder(
            command.UserId,
            command.SubscriptionId,
            command.Text,
            command.DateTime);

        return await _usersRepository.GetBySubscriptionIdAsync(command.SubscriptionId, cancellationToken)
            .ToResultAsync(Error.NotFound("Subscription not found"))
            .BindAsync(user => user.SetReminder(reminder).Map(r => user))
            .MapAsync(user =>
            {
                _usersRepository.UpdateAsync(user, cancellationToken);
                return reminder;
            });
    }
}