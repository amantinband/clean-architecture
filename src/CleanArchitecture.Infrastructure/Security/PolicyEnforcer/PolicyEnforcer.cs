using CleanArchitecture.Application.Common.Security.Policies;
using CleanArchitecture.Application.Common.Security.Request;
using CleanArchitecture.Application.Common.Security.Roles;
using CleanArchitecture.Infrastructure.Security.CurrentUserProvider;

using ErrorOr;

namespace CleanArchitecture.Infrastructure.Security.PolicyEnforcer;

public class PolicyEnforcer : IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(
        IAuthorizeableRequest<T> request,
        CurrentUser currentUser,
        string policy)
    {
        return request switch
        {
            IUserAuthorizeableRequest<T> userAuthorizeableRequest => UserPolicyEnforcer.Authorize(
                userAuthorizeableRequest,
                currentUser,
                policy),
            _ => Result.Success,
        };
    }
}