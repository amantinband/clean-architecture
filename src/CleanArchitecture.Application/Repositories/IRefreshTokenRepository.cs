using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CleanArchitecture.Domain.Auth;

namespace CleanArchitecture.Application.Repositories;

public interface IRefreshTokenRepository
{
    Task SaveAsync(string tokenHash, RefreshToken data, TimeSpan ttl);
    Task<RefreshToken?> GetAsync(string tokenHash);
    Task RevokeAsync(string tokenHash);
}
