using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Reminders;

namespace CleanArchitecture.Domain.Users.Events;

public record ReminderSetEvent(Reminder Reminder) : IDomainEvent;