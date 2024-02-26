using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users.Events;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Events;

public class SubscriptionCanceledEventHandler(IUsersRepository _usersRepository)
    : INotificationHandler<DomainEventNotification<SubscriptionCanceledEvent>>
{
    public async Task Handle(DomainEventNotification<SubscriptionCanceledEvent> notification, CancellationToken cancellationToken)
    {
        var @event = notification.DomainEvent;
        @event.User.DeleteAllReminders();

        await _usersRepository.RemoveAsync(@event.User, cancellationToken);
    }
}
