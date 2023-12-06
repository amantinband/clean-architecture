using CleanArchitecture.Contracts.Common;

namespace CleanArchitecture.Contracts.Tokens;

public record GenerateTokenRequest(
    Guid? Id,
    string FirstName,
    string LastName,
    string Email,
    SubscriptionType SubscriptionType,
    List<string> Permissions,
    List<string> Roles);