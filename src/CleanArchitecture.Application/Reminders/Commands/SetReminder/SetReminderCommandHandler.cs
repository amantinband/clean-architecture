using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.SetReminder;

public class SetReminderCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<SetReminderCommand, Result<Reminder>>
{
    public async Task<Result<Reminder>> Handle(SetReminderCommand command, CancellationToken cancellationToken) =>
        await _usersRepository.GetBySubscriptionIdAsync(command.SubscriptionId, cancellationToken)
            .ToResultAsync(Error.NotFound("Subscription not found"))
            .CombineAsync(Reminder.TryCreate(command.UserId, command.SubscriptionId, command.Text, command.DateTime))
            .BindAsync((user, reminder) => user.SetReminder(reminder).Map(r => (user, reminder)))
            .BindAsync((user, reminder) =>
            {
                _usersRepository.UpdateAsync(user, cancellationToken);
                return Result.Success(reminder);
            });
}