namespace CleanArchitecture.Domain.Users.Events;

public record SubscriptionCanceledEvent(User User, Guid SubscriptionId) : IDomainEvent;