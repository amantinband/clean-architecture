using CleanArchitecture.Application.Subscriptions.Commands.CancelSubscription;
using CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;
using CleanArchitecture.Contracts.Subscriptions;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using DomainSubscriptionType = CleanArchitecture.Domain.Users.SubscriptionType;
using SubscriptionType = CleanArchitecture.Contracts.Common.SubscriptionType;

namespace CleanArchitecture.Api.Controllers;

[Route("users/{userId:guid}/subscriptions")]
public class SubscriptionsController(IMediator _mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateSubscription(Guid userId, CreateSubscriptionRequest request) =>
        await DomainSubscriptionType.TryFromName(request.SubscriptionType.ToString(), out var subscriptionType).ToErrorOr()
            .When(val => val is false, Error.Validation("Invalid plan type"))
            .Map(_ => new CreateSubscriptionCommand(
                userId,
                request.FirstName,
                request.LastName,
                request.Email,
                subscriptionType))
            .MapAsync(command => _mediator.Send(command))
            .Match(
                subscription => CreatedAtAction(
                    actionName: nameof(GetSubscription),
                    routeValues: new { UserId = userId },
                    value: ToDto(subscription)),
                Problem);

    [HttpDelete("{subscriptionId:guid}")]
    public async Task<IActionResult> DeleteSubscription(Guid userId, Guid subscriptionId) =>
        await new CancelSubscriptionCommand(userId, subscriptionId).ToErrorOr()
            .MapAsync(command => _mediator.Send(command))
            .Match(_ => NoContent(), Problem);

    [HttpGet]
    public async Task<IActionResult> GetSubscription(Guid userId) =>
        await new GetSubscriptionQuery(userId).ToErrorOr()
            .MapAsync(query => _mediator.Send(query))
            .Map(ToDto)
            .Match(Ok, Problem);

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
            ToDto(subscriptionResult.SubscriptionType));
}