using CleanArchitecture.Application.Common.Security.Request;
using CleanArchitecture.Infrastructure.Security.CurrentUserProvider;

using ErrorOr;

namespace CleanArchitecture.Infrastructure.Security.PolicyEnforcer;

public interface IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(
        IAuthorizeableRequest<T> request,
        CurrentUser currentUser,
        string policy);
}