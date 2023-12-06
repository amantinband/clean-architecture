using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CleanArchitecture.Domain.Users;

using GymManagement.Application.Common.Interfaces;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GymManagement.Api.Authentication.TokenGenerator;

public class JwtTokenGenerator(IOptions<JwtSettings> jwtOptions) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string GenerateToken(
        Guid id,
        string firstName,
        string lastName,
        string email,
        SubscriptionType subscriptionType,
        List<string> permissions,
        List<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, firstName),
            new(JwtRegisteredClaimNames.FamilyName, lastName),
            new(JwtRegisteredClaimNames.Email, email),
            new("id", id.ToString()),
        };

        roles.ForEach(role => claims.Add(new(ClaimTypes.Role, role)));
        permissions.ForEach(permission => claims.Add(new("permissions", permission)));

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}