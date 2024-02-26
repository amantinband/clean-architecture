using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users.Events;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Events;

public class ReminderDeletedEventHandler(IRemindersRepository _remindersRepository) : INotificationHandler<DomainEventNotification<ReminderDeletedEvent>>
{
    public async Task Handle(DomainEventNotification<ReminderDeletedEvent> notification, CancellationToken cancellationToken)
    {
        var @event = notification.DomainEvent;
        var reminder = await _remindersRepository.GetByIdAsync(@event.ReminderId, cancellationToken)
            ?? throw new InvalidOperationException();

        await _remindersRepository.RemoveAsync(reminder, cancellationToken);
    }
}
