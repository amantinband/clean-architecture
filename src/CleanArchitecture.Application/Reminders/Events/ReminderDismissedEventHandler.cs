using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users.Events;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Events;

public class ReminderDismissedEventHandler(IRemindersRepository _remindersRepository) : INotificationHandler<ReminderDismissedEvent>
{
    public async Task Handle(ReminderDismissedEvent notification, CancellationToken cancellationToken)
    {
        await (await _remindersRepository.GetByIdAsync(notification.ReminderId, cancellationToken)).ToErrorOr()
            .FailIf(reminder => reminder is null, Error.Unexpected())
            .Then(reminder => reminder!.Dismiss().Then(success => reminder!))
            .ThenDoAsync(reminder => _remindersRepository.UpdateAsync(reminder, cancellationToken));
    }
}
