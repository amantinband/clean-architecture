using CleanArchitecture.Contracts.Tokens;

namespace CleanArchitecture.Api.IntegrationTests.Common.Tokens;

public static class TokenRequestFactory
{
    public static GenerateTokenRequest CreateGenerateTokenRequest(
        Guid? id = null,
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string email = Constants.User.Email,
        SubscriptionType? subscriptionType = null,
        List<string>? permissions = null,
        List<string>? roles = null)
    {
        return new(
            id ?? Constants.User.Id,
            firstName,
            lastName,
            email,
            subscriptionType ?? SubscriptionType.Basic,
            permissions ?? Constants.User.Permissions,
            roles ?? Constants.User.Roles);
    }
}