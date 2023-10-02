using CleanArchitecture.Domain.Reminders;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Queries.GetReminder;

public record GetReminderQuery(Guid ReminderId) : IRequest<ErrorOr<Reminder>>;
