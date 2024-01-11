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
        return await (await _usersRepository.GetBySubscriptionIdAsync(command.SubscriptionId, cancellationToken)).ToErrorOr()
            .FailIf(user => user is null, Error.NotFound(description: "Subscription not found"))
            .Then(user =>
            {
                var reminder = new Reminder(
                    command.UserId,
                    command.SubscriptionId,
                    command.Text,
                    command.DateTime);

                return user!.SetReminder(reminder)
                    .Then(success => (User: user!, Reminder: reminder));
            })
            .ThenDoAsync(pair => _usersRepository.UpdateAsync(pair.User, cancellationToken))
            .Then(pair => pair.Reminder);
    }
}