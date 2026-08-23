using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Repositories;
using CleanArchitecture.Domain.Auth;
using CleanArchitecture.Domain.Users;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Security.TokenGenerator;

public class JwtTokenGenerator(IOptions<JwtSettings> jwtOptions, IRefreshTokenRepository refreshTokenRepository, IUsersRepository usersRepository) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUsersRepository _usersRepository = usersRepository;

    public async Task<string> GenerateTokenAsync(Guid userId, IEnumerable<string>? permissions = null, IEnumerable<string>? roles = null, CancellationToken cancellationToken = default)
    {
        var user = await _usersRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("id", user.Id.ToString()),
        };

        if (roles != null)
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        if (permissions != null)
        {
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permissions", permission));
            }
        }

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken, Guid userId, IEnumerable<string>? permissions = null, IEnumerable<string>? roles = null, CancellationToken cancellationToken = default)
    {
        var tokenHash = HashRefreshToken(refreshToken);
        var existingRefreshToken = await _refreshTokenRepository.GetAsync(tokenHash);
        if (existingRefreshToken == null || existingRefreshToken.UserId != userId)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        // Generate a new access token and refresh token
        var newAccessToken = await GenerateTokenAsync(userId, permissions, roles, cancellationToken);
        var newRefreshToken = Guid.NewGuid().ToString();

        // Save the new refresh token with a TTL
        var newRefreshTokenHash = HashRefreshToken(newRefreshToken);
        await _refreshTokenRepository.SaveAsync(newRefreshTokenHash, new RefreshToken { UserId = userId }, TimeSpan.FromDays(7));

        // Revoke the old refresh token
        await _refreshTokenRepository.RevokeAsync(tokenHash);

        return (newAccessToken, newRefreshToken);
    }

    private string HashRefreshToken(string refreshToken)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(refreshToken);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
