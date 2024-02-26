using MediatR;

namespace CleanArchitecture.Application
{
    public class DomainEventNotification<TDomainEvent>(TDomainEvent domainEvent) : INotification
        where TDomainEvent : IDomainEvent
    {
        public TDomainEvent DomainEvent { get; } = domainEvent;
    }
}
