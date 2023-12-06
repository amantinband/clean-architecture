namespace CleanArchitecture.Contracts.Users;

public record CreateUserRequest(Guid Id, string FirstName, string LastName, SubscriptionType SubscriptionType);