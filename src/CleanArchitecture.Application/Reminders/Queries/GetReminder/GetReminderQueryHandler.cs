using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Queries.GetReminder;

public class GetReminderQueryHandler(IRemindersRepository _remindersRepository)
        : IRequestHandler<GetReminderQuery, Result<Reminder>>
{
    public async Task<Result<Reminder>> Handle(GetReminderQuery query, CancellationToken cancellationToken)
    {
        var reminder = await _remindersRepository.GetByIdAsync(query.ReminderId, cancellationToken);

        if (reminder?.UserId != query.UserId)
        {
            return Error.NotFound("Reminder not found");
        }

        return reminder;
    }
}