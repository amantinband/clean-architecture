using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Infrastructure.Common;

namespace CleanArchitecture.Infrastructure.Reminders.Persistence;

public class RemindersRepository(AppDbContext _dbContext) : IRemindersRepository
{
    public async Task AddReminderAsync(Reminder reminder)
    {
        await _dbContext.AddAsync(reminder);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Reminder?> GetReminderByIdAsync(Guid reminderId)
    {
        return await _dbContext.Reminders.FindAsync(reminderId);
    }
}
