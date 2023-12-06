using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Authentication.Queries.Login;

public record GenerateTokenResult(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    SubscriptionType SubscriptionType,
    string Token);