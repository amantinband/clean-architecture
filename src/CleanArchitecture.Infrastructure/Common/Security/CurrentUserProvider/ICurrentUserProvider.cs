using CleanArchitecture.Infrastructure.Common.Security.CurrentUserProvider;

namespace CleanArchitecture.Infrastructure.Security.CurrentUserProvider;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}