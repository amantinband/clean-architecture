using CleanArchitecture.Domain.Reminders;

namespace CleanArchitecture.Domain.Users.Events;

public record ReminderDeletedEvent(ReminderId ReminderId) : IDomainEvent;