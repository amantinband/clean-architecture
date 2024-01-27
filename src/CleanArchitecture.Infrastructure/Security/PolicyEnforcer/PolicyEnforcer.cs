using CleanArchitecture.Application.Common.Security.Policies;
using CleanArchitecture.Application.Common.Security.Request;
using CleanArchitecture.Application.Common.Security.Roles;
using CleanArchitecture.Infrastructure.Security.CurrentUserProvider;

namespace CleanArchitecture.Infrastructure.Security.PolicyEnforcer;

public class PolicyEnforcer : IPolicyEnforcer
{
    public Result<Unit> Authorize<T>(
        IAuthorizeableRequest<T> request,
        CurrentUser currentUser,
        string policy)
    {
        return policy switch
        {
            Policy.SelfOrAdmin => SelfOrAdminPolicy(request, currentUser),
            _ => Error.Unexpected("Unknown policy name"),
        };
    }

    private static Result<Unit> SelfOrAdminPolicy<T>(IAuthorizeableRequest<T> request, CurrentUser currentUser) =>
        request.UserId == currentUser.Id || currentUser.Roles.Contains(Role.Admin)
            ? Result.Success()
            : Error.Forbidden("Requesting user failed policy requirement");
}