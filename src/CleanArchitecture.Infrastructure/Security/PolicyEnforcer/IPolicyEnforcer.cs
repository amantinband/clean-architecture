using CleanArchitecture.Application.Common.Security.Request;
using CleanArchitecture.Infrastructure.Security.CurrentUserProvider;

namespace CleanArchitecture.Infrastructure.Security.PolicyEnforcer;

public interface IPolicyEnforcer
{
    public Result<Unit> Authorize<T>(
        IAuthorizeableRequest<T> request,
        CurrentUser currentUser,
        string policy);
}