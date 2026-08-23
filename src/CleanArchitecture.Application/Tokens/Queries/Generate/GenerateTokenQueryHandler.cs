using CleanArchitecture.Application.Authentication.Queries.Login;
using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tokens.Queries.Generate;

public class GenerateTokenQueryHandler(IJwtTokenGenerator _jwtTokenGenerator) : IRequestHandler<GenerateTokenQuery, ErrorOr<GenerateTokenResult>>
{
    public async Task<ErrorOr<GenerateTokenResult>> Handle(GenerateTokenQuery query, CancellationToken cancellationToken)
    {
        var id = query.Id ?? Guid.NewGuid();

        var token = await _jwtTokenGenerator.GenerateTokenAsync(id, query.Permissions, query.Roles, cancellationToken);

        var authResult = new GenerateTokenResult(
            id,
            query.FirstName,
            query.LastName,
            query.Email,
            query.SubscriptionType,
            token);

        return ErrorOrFactory.From(authResult);
    }
}
