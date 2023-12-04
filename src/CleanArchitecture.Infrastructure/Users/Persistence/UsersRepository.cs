using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Common;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Users.Persistence;

public class UsersRepository(AppDbContext _dbContext) : IUsersRepository
{
    public async Task AddUserAsync(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}