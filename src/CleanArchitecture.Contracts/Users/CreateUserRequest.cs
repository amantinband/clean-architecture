namespace CleanArchitecture.Contracts.Users;

public record CreateUserRequest(string FullName, PlanType PlanType);