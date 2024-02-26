using CleanArchitecture.Domain.Reminders;

namespace CleanArchitecture.Domain.Users.Events;

public record ReminderDismissedEvent(ReminderId ReminderId) : IDomainEvent;