using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Users.Events;

public record ReminderDeletedEvent(Guid ReminderId) : IDomainEvent;