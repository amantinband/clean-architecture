using CleanArchitecture.Infrastructure.Common.Security.CurrentUserProvider;

using TestCommon.TestConstants;

namespace TestCommon.Security;

public static class CurrentUserFactory
{
    public static CurrentUser CreateCurrentUser(
        Guid? id = null,
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string email = Constants.User.Email,
        IReadOnlyList<string>? permissions = null,
        IReadOnlyList<string>? roles = null)
    {
        return new CurrentUser(
            id ?? Constants.User.Id,
            firstName,
            lastName,
            email,
            permissions ?? Constants.User.Permissions,
            roles ?? Constants.User.Permissions);
    }
}