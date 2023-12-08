using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Users.Events;

public record SubscriptionDeletedEvent(User User, Guid SubscriptionId) : IDomainEvent;