using CleanArchitecture.Application.Authentication.Queries.Login;
using CleanArchitecture.Application.Tokens.Queries.Generate;
using CleanArchitecture.Contracts.Common;
using CleanArchitecture.Contracts.Tokens;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using DomainSubscriptionType = CleanArchitecture.Domain.Users.SubscriptionType;

namespace CleanArchitecture.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("tokens")]
public class TokensController(ISender _mediator) : ControllerBase
{
    [HttpPost("generate")]
    public async Task<ActionResult<TokenResponse>> GenerateToken(GenerateTokenRequest request)
    {
        if (!DomainSubscriptionType.TryFromName(request.SubscriptionType.ToString(), out var plan))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: "Invalid subscription type");
        }

        var query = new GenerateTokenQuery(
            request.Id,
            request.FirstName,
            request.LastName,
            request.Email,
            plan,
            request.Permissions,
            request.Roles);

        return await _mediator.Send(query)
            .MapAsync(ToDto)
            .ToOkActionResultAsync(this);
    }

    private static TokenResponse ToDto(GenerateTokenResult authResult)
    {
        return new TokenResponse(
            authResult.Id,
            authResult.FirstName,
            authResult.LastName,
            authResult.Email,
            ToDto(authResult.SubscriptionType),
            authResult.Token);
    }

    private static SubscriptionType ToDto(DomainSubscriptionType subscriptionType) =>
        subscriptionType.Name switch
        {
            nameof(DomainSubscriptionType.Basic) => SubscriptionType.Basic,
            nameof(DomainSubscriptionType.Pro) => SubscriptionType.Pro,
            _ => throw new InvalidOperationException(),
        };
}