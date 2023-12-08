using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Infrastructure.Security.CurrentUserProvider;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}