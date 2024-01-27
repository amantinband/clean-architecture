using CleanArchitecture.Application.Authentication.Queries.Login;
using CleanArchitecture.Application.Common.Interfaces;

using MediatR;

namespace CleanArchitecture.Application.Tokens.Queries.Generate;

public class GenerateTokenQueryHandler(
    IJwtTokenGenerator _jwtTokenGenerator)
        : IRequestHandler<GenerateTokenQuery, Result<GenerateTokenResult>>
{
    public Task<Result<GenerateTokenResult>> Handle(GenerateTokenQuery query, CancellationToken cancellationToken)
    {
        var id = query.Id ?? Guid.NewGuid();

        var token = _jwtTokenGenerator.GenerateToken(
            id,
            query.FirstName,
            query.LastName,
            query.Email,
            query.SubscriptionType,
            query.Permissions,
            query.Roles);

        var authResult = new GenerateTokenResult(
            id,
            query.FirstName,
            query.LastName,
            query.Email,
            query.SubscriptionType,
            token);

        return Task.FromResult(Result.Success(authResult));
    }
}