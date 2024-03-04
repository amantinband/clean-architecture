using CleanArchitecture.Application.Common.Security.Policies;
using CleanArchitecture.Application.Common.Security.Request;
using CleanArchitecture.Application.Common.Security.Roles;
using CleanArchitecture.Infrastructure.Security.CurrentUserProvider;

using ErrorOr;

namespace CleanArchitecture.Infrastructure.Security.PolicyEnforcer;

public static class UserPolicyEnforcer
{
    public static ErrorOr<Success> Authorize<T>(
        IUserAuthorizeableRequest<T> request,
        CurrentUser currentUser,
        string policy)
    {
        return policy switch
        {
            Policy.SelfOrAdmin => SelfOrAdminPolicy(request, currentUser),
            _ => Error.Unexpected(description: "Unknown policy name"),
        };
    }

    private static ErrorOr<Success> SelfOrAdminPolicy<T>(IUserAuthorizeableRequest<T> request, CurrentUser currentUser) =>
        request.UserId == currentUser.Id || currentUser.Roles.Contains(Role.Admin)
            ? Result.Success
            : Error.Unauthorized(description: "Requesting user failed policy requirement");
}