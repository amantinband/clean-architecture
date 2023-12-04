namespace CleanArchitecture.Contracts.Users;

public record UserResponse(
    Guid Id,
    string FullName,
    PlanType PlanType);