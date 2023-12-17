namespace CleanArchitecture.Infrastructure.Common.Security.CurrentUserProvider;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}