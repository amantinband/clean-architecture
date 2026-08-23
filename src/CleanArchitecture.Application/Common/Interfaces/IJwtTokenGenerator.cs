using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    Task<string> GenerateTokenAsync(Guid userId, IEnumerable<string>? permissions = null, IEnumerable<string>? roles = null, CancellationToken cancellationToken = default);

    Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken, Guid userId, IEnumerable<string>? permissions = null, IEnumerable<string>? roles = null, CancellationToken cancellationToken = default);
}
