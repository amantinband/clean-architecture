using CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

using FluentAssertions;
using MediatR;

namespace TestCommon.Subscriptions;

public static class MediatorExtensions
{
    public static async Task<SubscriptionResult> CreateSubscriptionAsync(
        this IMediator mediator,
        CreateSubscriptionCommand? command = null)
    {
        command ??= SubscriptionCommandFactory.CreateCreateSubscriptionCommand().Value;

        var result = await mediator.Send(command);

        result.IsFailure.Should().BeFalse();
        result.Value.AssertCreatedFrom(command);

        return result.Value;
    }

    public static async Task<Result<SubscriptionResult>> GetSubscriptionAsync(
        this IMediator mediator,
        GetSubscriptionQuery? query = null)
    {
        return await mediator.Send(query ?? SubscriptionQueryFactory.CreateGetSubscriptionQuery());
    }
}