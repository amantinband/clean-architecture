using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IUsersRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetByIdAsync(Guid userId);
    Task UpdateAsync(User user);
}