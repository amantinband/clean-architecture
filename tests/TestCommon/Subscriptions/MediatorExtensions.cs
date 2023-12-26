using CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

using ErrorOr;

using FluentAssertions;

using MediatR;

namespace TestCommon.Subscriptions;

public static class MediatorExtensions
{
    public static async Task<SubscriptionResult> CreateSubscriptionAsync(
        this IMediator mediator,
        CreateSubscriptionCommand? command = null)
    {
        command ??= SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        var result = await mediator.Send(command);

        result.IsError.Should().BeFalse();
        result.Value.AssertCreatedFrom(command);

        return result.Value;
    }

    public static async Task<ErrorOr<SubscriptionResult>> GetSubscriptionAsync(
        this IMediator mediator,
        GetSubscriptionQuery? query = null)
    {
        return await mediator.Send(query ?? SubscriptionQueryFactory.CreateGetSubscriptionQuery());
    }
}