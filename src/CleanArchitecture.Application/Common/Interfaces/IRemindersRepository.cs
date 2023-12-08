using CleanArchitecture.Domain.Reminders;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IRemindersRepository
{
    Task AddAsync(Reminder reminder, CancellationToken cancellationToken);
    Task<Reminder?> GetByIdAsync(Guid reminderId, CancellationToken cancellationToken);
    Task<List<Reminder>> ListBySubscriptionIdAsync(Guid subscriptionId, CancellationToken cancellationToken);
    Task RemoveAsync(Reminder reminder, CancellationToken cancellationToken);
    Task RemoveRangeAsync(List<Reminder> reminders, CancellationToken cancellationToken);
    Task UpdateAsync(Reminder reminder, CancellationToken cancellationToken);
}