using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IUsersRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<User?> GetBySubscriptionIdAsync(Guid subscriptionId, CancellationToken cancellationToken);
    Task<IEnumerable<User?>> GetByEmailsAsync(List<string> emails);
    Task<User?> GetByEmailAsync(string email);
    Task RemoveAsync(User user, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
}