using CleanArchitecture.Application;
using MediatR;

namespace CleanArchitecture.Infrastructure.Common
{
    internal static class DomainEventExtensions
    {
        public static INotification GetNotificationEvent(this IDomainEvent @event)
        {
            var eventType = @event.GetType();

            var notification =
                Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(eventType), @event) as
                    INotification;

            return notification!;
        }
    }
}
