
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Common.Persistence;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Users.Persistence;

public class UsersRepository(AppDbContext _dbContext) : IUsersRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<User?>> GetByEmailsAsync(List<string> emails)
    {
        return await _dbContext.Users
            .Include(user => user.Subscription)
            .Where(user => emails.Contains(user.Email)).ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users
        .Include(user => user.Subscription)
        .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FindAsync(userId, cancellationToken);
    }

    public async Task<User?> GetBySubscriptionIdAsync(Guid subscriptionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Subscription.Id == subscriptionId, cancellationToken);
    }

    public async Task RemoveAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}