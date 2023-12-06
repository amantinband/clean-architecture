using System.Reflection;

using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using GymManagement.Application.Common.Authorization;

using MediatR;

namespace GymManagement.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(ICurrentUserProvider _currentUserProvider)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        var currentUser = _currentUserProvider.GetCurrentUser();

        var requiredPermissions = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Permissions?.Split(',') ?? [])
            .ToList();

        if (requiredPermissions.Except(currentUser.Permissions).Any())
        {
            return (dynamic)Error.Unauthorized(description: "User is forbidden from taking this action");
        }

        var requiredRoles = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? [])
            .ToList();

        if (requiredRoles.Except(currentUser.Roles).Any())
        {
            return (dynamic)Error.Unauthorized(description: "User is forbidden from taking this action");
        }

        return await next();
    }
}
