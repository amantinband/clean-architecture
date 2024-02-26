using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IUsersRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<User?> GetBySubscriptionIdAsync(Guid subscriptionId, CancellationToken cancellationToken);
    Task RemoveAsync(User user, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
}