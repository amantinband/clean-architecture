using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users.Events;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Events;

public class ReminderDismissedEventHandler(IRemindersRepository _remindersRepository) : INotificationHandler<DomainEventNotification<ReminderDismissedEvent>>
{
    public async Task Handle(DomainEventNotification<ReminderDismissedEvent> notification, CancellationToken cancellationToken)
    {
        var @event = notification.DomainEvent;
        var reminder = await _remindersRepository.GetByIdAsync(@event.ReminderId, cancellationToken)
            ?? throw new InvalidOperationException();

        reminder.Dismiss();

        await _remindersRepository.UpdateAsync(reminder, cancellationToken);
    }
}
