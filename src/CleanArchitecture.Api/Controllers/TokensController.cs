using CleanArchitecture.Application.Authentication.Queries.Login;
using CleanArchitecture.Application.Tokens.Queries.Generate;
using CleanArchitecture.Contracts.Common;
using CleanArchitecture.Contracts.Tokens;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using DomainSubscriptionType = CleanArchitecture.Domain.Users.SubscriptionType;

namespace CleanArchitecture.Api.Controllers;

[Route("tokens")]
[AllowAnonymous]
public class TokensController(ISender _mediator) : ApiController
{
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateToken(GenerateTokenRequest request) =>
        await DomainSubscriptionType.TryFromName(request.SubscriptionType.ToString(), out var plan).ToErrorOr()
            .When(val => val is false, Error.Validation("Invalid subscription type"))
            .Map(_ => new GenerateTokenQuery(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Email,
                plan,
                request.Permissions,
                request.Roles))
            .MapAsync(query => _mediator.Send(query))
            .Map(ToDto)
            .Match(Ok, Problem);

    private static ErrorOr<TokenResponse> ToDto(GenerateTokenResult authResult) =>
        new TokenResponse(
            authResult.Id,
            authResult.FirstName,
            authResult.LastName,
            authResult.Email,
            ToDto(authResult.SubscriptionType),
            authResult.Token);

    private static SubscriptionType ToDto(DomainSubscriptionType subscriptionType) =>
        subscriptionType.Name switch
        {
            nameof(DomainSubscriptionType.Basic) => SubscriptionType.Basic,
            nameof(DomainSubscriptionType.Pro) => SubscriptionType.Pro,
            _ => throw new InvalidOperationException(),
        };
}