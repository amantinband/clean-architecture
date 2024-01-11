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
            .When(reminder => reminder is null, Error.Unexpected())
            .Map(reminder => reminder!.Dismiss().Map(success => reminder!))
            .TapAsync(reminder => _remindersRepository.UpdateAsync(reminder, cancellationToken));
    }
}
