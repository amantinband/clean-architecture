namespace CleanArchitecture.Contracts.Users;

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    SubscriptionType SubscriptionType);