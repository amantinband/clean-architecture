using CleanArchitecture.Domain.Reminders;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IRemindersRepository
{
    Task AddAsync(Reminder reminder);
    Task<Reminder?> GetByIdAsync(Guid reminderId);
}