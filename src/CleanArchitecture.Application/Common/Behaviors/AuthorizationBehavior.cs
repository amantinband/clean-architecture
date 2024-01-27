using System.Reflection;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Security.Request;

using MediatR;

namespace CleanArchitecture.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(
    IAuthorizationService _authorizationService)
        : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IAuthorizeableRequest<TResponse>
            where TResponse : IResult
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

        var requiredPermissions = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Permissions?.Split(',') ?? [])
            .ToList();

        var requiredRoles = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? [])
            .ToList();

        var requiredPolicies = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Policies?.Split(',') ?? [])
            .ToList();

        var authorizationResult = _authorizationService.AuthorizeCurrentUser(
            request,
            requiredRoles,
            requiredPermissions,
            requiredPolicies);

        return authorizationResult.IsFailure
            ? (dynamic)authorizationResult.Error
            : await next();
    }
}