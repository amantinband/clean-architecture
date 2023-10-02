using CleanArchitecture.Domain.Reminders;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IRemindersRepository
{
    Task AddReminderAsync(Reminder reminder);
    Task<Reminder?> GetReminderByIdAsync(Guid reminderId);
}