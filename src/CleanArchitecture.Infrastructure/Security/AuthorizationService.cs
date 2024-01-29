using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Security.Request;
using CleanArchitecture.Infrastructure.Security.CurrentUserProvider;
using CleanArchitecture.Infrastructure.Security.PolicyEnforcer;

namespace CleanArchitecture.Infrastructure.Security;

public class AuthorizationService(
    IPolicyEnforcer _policyEnforcer,
    ICurrentUserProvider _currentUserProvider)
        : IAuthorizationService
{
    public Result<Unit> AuthorizeCurrentUser<T>(
        IAuthorizeableRequest<T> request,
        List<string> requiredRoles,
        List<string> requiredPermissions,
        List<string> requiredPolicies)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();

        if (requiredPermissions.Except(currentUser.Permissions).Any())
        {
            return Error.Forbidden("User is missing required permissions for taking this action");
        }

        if (requiredRoles.Except(currentUser.Roles).Any())
        {
            return Error.Forbidden("User is missing required roles for taking this action");
        }

        foreach (var policy in requiredPolicies)
        {
            var authorizationAgainstPolicyResult = _policyEnforcer.Authorize(request, currentUser, policy);

            if (authorizationAgainstPolicyResult.IsFailure)
            {
                return authorizationAgainstPolicyResult.Error;
            }
        }

        return Result.Success();
    }
}