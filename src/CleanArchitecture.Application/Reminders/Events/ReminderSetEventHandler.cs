using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users.Events;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Events;

public class ReminderSetEventHandler(IRemindersRepository _remindersRepository) : INotificationHandler<DomainEventNotification<ReminderSetEvent>>
{
    public async Task Handle(DomainEventNotification<ReminderSetEvent> notification, CancellationToken cancellationToken)
    {
        var @event = notification.DomainEvent;
        await _remindersRepository.AddAsync(@event.Reminder, cancellationToken);
    }
}
