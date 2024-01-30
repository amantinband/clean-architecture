using CleanArchitecture.Application.Authentication.Queries.Login;
using CleanArchitecture.Domain.Users;

using MediatR;

namespace CleanArchitecture.Application.Tokens.Queries.Generate;

public class GenerateTokenQuery : IRequest<Result<GenerateTokenResult>>
{
    public Guid? Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public SubscriptionType SubscriptionType { get; }
    public List<string> Permissions { get; }
    public List<string> Roles { get; }

    public static Result<GenerateTokenQuery> TryCreate(Guid? id, string firstName, string lastName, string email, string subscriptionType, List<string> permissions, List<string> roles) =>
        SubscriptionType.TryFromName(subscriptionType, out var plan)
            ? new GenerateTokenQuery(id, firstName, lastName, email, plan, permissions, roles)
            : Error.Validation("Invalid subscription type", nameof(subscriptionType));

    private GenerateTokenQuery(Guid? id, string firstName, string lastName, string email, SubscriptionType subscriptionType, List<string> permission, List<string> roles)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        SubscriptionType = subscriptionType;
        Permissions = permission;
        Roles = roles;
    }
}