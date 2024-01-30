using CleanArchitecture.Application.Subscriptions.Commands.CancelSubscription;
using CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;
using CleanArchitecture.Contracts.Subscriptions;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using DomainSubscriptionType = CleanArchitecture.Domain.Users.SubscriptionType;
using SubscriptionType = CleanArchitecture.Contracts.Common.SubscriptionType;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Authorize]
[Route("users/{userId:guid}/subscriptions")]
public class SubscriptionsController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSubscription(Guid userId, CreateSubscriptionRequest request)
    {
        IConvertToActionResult actionResult = await CreateSubscriptionCommand.TryCreate(
            userId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.SubscriptionType.ToString())
            .BindAsync(command => _mediator.Send(command))
            .FinallyAsync(
             subscription => CreatedAtAction(
                actionName: nameof(GetSubscription),
                routeValues: new { UserId = userId },
                value: ToDto(subscription)),
             err => err.ToErrorActionResult<SubscriptionResult>(this));

        return actionResult.Convert();
    }

    [HttpDelete("{subscriptionId:guid}")]
    public async Task<ActionResult<FunctionalDdd.Unit>> DeleteSubscription(Guid userId, Guid subscriptionId) =>
        await new CancelSubscriptionCommand(userId, subscriptionId).ToResult()
            .BindAsync(command => _mediator.Send(command))
            .FinallyAsync(
              _ => NoContent(),
              err => err.ToErrorActionResult<FunctionalDdd.Unit>(this));

    [HttpGet]
    public async Task<ActionResult<SubscriptionResponse>> GetSubscription(Guid userId) =>
        await new GetSubscriptionQuery(userId).ToResult()
            .BindAsync(query => _mediator.Send(query))
            .MapAsync(ToDto)
            .ToOkActionResultAsync(this);

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