using CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;
using CleanArchitecture.Contracts.Subscriptions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using DomainSubscriptionType = CleanArchitecture.Domain.Users.SubscriptionType;
using SubscriptionType = CleanArchitecture.Contracts.Common.SubscriptionType;

namespace CleanArchitecture.Api.Controllers;

[Route("subscriptions")]
public class SubscriptionsController(IMediator _mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
    {
        if (!DomainSubscriptionType.TryFromName(request.SubscriptionType.ToString(), out var subscriptionType))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: "Invalid plan type");
        }

        var createSubscriptionCommand = new CreateSubscriptionCommand(subscriptionType);

        var createSubscription = await _mediator.Send(createSubscriptionCommand);

        return createSubscription.Match(
            subscriptionResult => CreatedAtAction(
                actionName: nameof(GetSubscription),
                routeValues: new { SubscriptionId = subscriptionResult.Id },
                value: ToDto(subscriptionResult)),
            Problem);
    }

    [HttpGet]
    public async Task<IActionResult> GetSubscription()
    {
        var getSubscriptionQuery = new GetSubscriptionQuery();

        var getUserResult = await _mediator.Send(getSubscriptionQuery);

        return getUserResult.Match(
            user => Ok(ToDto(user)),
            Problem);
    }

    private static SubscriptionType ToDto(DomainSubscriptionType subscriptionType) =>
        subscriptionType.Name switch
        {
            nameof(DomainSubscriptionType.Basic) => SubscriptionType.Basic,
            nameof(DomainSubscriptionType.Pro) => SubscriptionType.Pro,
            _ => throw new InvalidOperationException(),
        };

    private static SubscriptionResponse ToDto(SubscriptionResult subscriptionResult) =>
        new(
            subscriptionResult.Id,
            subscriptionResult.UserId,
            subscriptionResult.UserFirstName,
            subscriptionResult.UserLastName,
            ToDto(subscriptionResult.SubscriptionType));
}