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
            .When(user => user is null, Error.NotFound(description: "Subscription not found"))
            .Map(user =>
            {
                var reminder = new Reminder(
                    command.UserId,
                    command.SubscriptionId,
                    command.Text,
                    command.DateTime);

                return user!.SetReminder(reminder)
                    .Map(success => (User: user!, Reminder: reminder));
            })
            .TapAsync(pair => _usersRepository.UpdateAsync(pair.User, cancellationToken))
            .Map(pair => pair.Reminder);
    }
}