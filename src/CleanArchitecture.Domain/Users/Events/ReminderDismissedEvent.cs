using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Users.Events;

public record ReminderDismissedEvent(Guid ReminderId) : IDomainEvent;