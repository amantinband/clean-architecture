using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users.Events;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Events;

public class ReminderDeletedEventHandler(IRemindersRepository _remindersRepository) : INotificationHandler<ReminderDeletedEvent>
{
    public async Task Handle(ReminderDeletedEvent notification, CancellationToken cancellationToken)
    {
        await (await _remindersRepository.GetByIdAsync(notification.ReminderId, cancellationToken)).ToErrorOr()
            .FailIf(reminder => reminder is null, Error.Unexpected())
            .SwitchAsync(
                reminder => _remindersRepository.RemoveAsync(reminder!, cancellationToken),
                _ => throw new InvalidOperationException());
    }
}
