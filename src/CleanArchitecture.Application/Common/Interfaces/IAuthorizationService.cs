using CleanArchitecture.Application.Common.Security.Request;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IAuthorizationService
{
    Result<Unit> AuthorizeCurrentUser<T>(
        IAuthorizeableRequest<T> request,
        List<string> requiredRoles,
        List<string> requiredPermissions,
        List<string> requiredPolicies);
}