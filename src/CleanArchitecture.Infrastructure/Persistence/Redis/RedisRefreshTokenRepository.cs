using System.Text.Json;

using CleanArchitecture.Application.Repositories;
using CleanArchitecture.Domain.Auth;

using StackExchange.Redis;

namespace CleanArchitecture.Infrastructure.Persistence.Redis;

public class RedisRefreshTokenRepository(IDatabase database) : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetAsync(string tokenHash)
    {
        var value = await database.StringGetAsync(tokenHash);
        if (!value.HasValue)
        {
            return null;
        }

        return JsonSerializer.Deserialize<RefreshToken>(value.ToString());
    }

    public Task RevokeAsync(string tokenHash)
    {
        database.KeyDeleteAsync(tokenHash);
        return Task.CompletedTask;
    }

    public Task SaveAsync(string tokenHash, RefreshToken data, TimeSpan ttl)
    {
        database.StringSetAsync(tokenHash, JsonSerializer.Serialize(data), ttl);
        return Task.CompletedTask;
    }
}
