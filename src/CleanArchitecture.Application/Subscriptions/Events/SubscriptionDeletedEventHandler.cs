using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users.Events;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Events;

public class SubscriptionDeletedEventHandler(IUsersRepository _usersRepository)
    : INotificationHandler<SubscriptionDeletedEvent>
{
    public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
    {
        notification.User.DeleteAllReminders();

        await _usersRepository.RemoveAsync(notification.User, cancellationToken);
    }
}
