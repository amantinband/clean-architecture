namespace CleanArchitecture.Infrastructure.Common.Security.CurrentUserProvider;

public record CurrentUser(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    IReadOnlyList<string> Permissions,
    IReadOnlyList<string> Roles);