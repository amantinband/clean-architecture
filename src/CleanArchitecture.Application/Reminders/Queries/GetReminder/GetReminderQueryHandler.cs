using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Queries.GetReminder;

public class GetReminderQueryHandler(
    IRemindersRepository _remindersRepository,
    ICurrentUserProvider _currentUserProvider)
        : IRequestHandler<GetReminderQuery, ErrorOr<Reminder>>
{
    public async Task<ErrorOr<Reminder>> Handle(GetReminderQuery query, CancellationToken cancellationToken)
    {
        var reminder = await _remindersRepository.GetReminderByIdAsync(query.ReminderId);

        if (reminder is null)
        {
            return Error.NotFound(description: "Reminder not found");
        }

        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser.Id != reminder.UserId)
        {
            return Error.Unauthorized(description: "User is unauthorized to fetch this reminder");
        }

        return reminder;
    }
}