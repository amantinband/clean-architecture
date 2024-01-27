using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Queries.ListReminders;

public class ListRemindersQueryHandler(IRemindersRepository _remindersRepository) : IRequestHandler<ListRemindersQuery, Result<List<Reminder>>>
{
    public async Task<Result<List<Reminder>>> Handle(ListRemindersQuery request, CancellationToken cancellationToken)
    {
        return await _remindersRepository.ListBySubscriptionIdAsync(request.SubscriptionId, cancellationToken);
    }
}
