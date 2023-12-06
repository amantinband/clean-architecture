using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Common.Models;

public record CurrentUser(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    IReadOnlyList<string> Permissions,
    IReadOnlyList<string> Roles);