using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Common;

namespace CleanArchitecture.Infrastructure.Users.Persistence;

public class UsersRepository(AppDbContext _dbContext) : IUsersRepository
{
    public async Task AddAsync(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _dbContext.Users.FindAsync(userId);
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Update(user);

        await _dbContext.SaveChangesAsync();
    }
}