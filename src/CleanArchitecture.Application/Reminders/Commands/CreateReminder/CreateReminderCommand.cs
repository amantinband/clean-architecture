using CleanArchitecture.Domain.Reminders;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.CreateReminder;

public record CreateReminderCommand(string Text, DateTimeOffset DateTime) : IRequest<ErrorOr<Reminder>>;